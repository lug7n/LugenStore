using LugenStore.Application.DTOs.Game;
using LugenStore.Application.Interfaces;
using LugenStore.Domain.Entities;
using LugenStore.Domain.Exceptions;
using LugenStore.Domain.Interfaces;
using LugenStore.Infrastructure.Persistence;
using System.Text.RegularExpressions;

namespace LugenStore.Application.Services
{
    public partial class GameService(IGameRepository _repository, IGenreRepository _genreRepository, IPublisherRepository _publisherRepository, IUnitOfWork _unitOfWork) : IGameService
    {
        private static void SanitizeInput(GameBaseDto dto)
        { 
            dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name, " ");
            dto.Description = GeneratedRegexes.WhitespaceRegex().Replace(dto.Description, " ");
        }

        public async Task<IEnumerable<GameResponseDto>> GetAllAsync()
        {
            var games = await _repository.GetAllAsync();

            return games.Select(game => new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price,
                Publishers = game.Publishers.Select(p => p.Name).ToList(),
                Genres = game.Genres.Select(g => g.Name).ToList(),
                Description = game.Description,
                CreatedAt = game.CreatedAt
            });
        }

        public async Task<GameResponseDto?> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty.");

            var game = await _repository.GetByIdAsync(id);

            if (game is null)
                throw new NotFoundException($"Game with id {id} not found.");

            return new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price,
                Publishers = game.Publishers.Select(p => p.Name).ToList(),
                Genres = game.Genres.Select(g => g.Name).ToList(),
                Description = game.Description,
                CreatedAt = game.CreatedAt
            };
        }

        public async Task<GameResponseDto> CreateAsync(CreateGameDto dto)
        {
            SanitizeInput(dto);

            var genres = new List<Genre>();
            var publishers = new List<Publisher>();

            if (dto.GenreId == null || dto.GenreId.Count == 0)
                throw new ValidationException("At least one genre must be provided.");

            foreach (var genreId in dto.GenreId)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);
                if (genre == null)
                    throw new NotFoundException($"Genre with id {genreId} not found.");
                genres.Add(genre);
            }

            foreach (var publisherId in dto.PublisherId)
            {
                var publisher = await _publisherRepository.GetByIdAsync(publisherId);
                if (publisher == null)
                    throw new NotFoundException($"Publisher with id {publisherId} not found.");
                publishers.Add(publisher);
            }

            if (await _repository.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException($"Game with name {dto.Name} already exists.");

            var game = new Game(dto.Name, dto.Price, dto.Description, publishers, genres);
           
            await _repository.CreateAsync(game);
            await _unitOfWork.SaveChangesAsync();
            
            return new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price,
                Description = game.Description,
                Publishers = game.Publishers.Select(p => p.Name).ToList(),
                Genres = game.Genres.Select(g => g.Name).ToList(),
                CreatedAt = game.CreatedAt,

            };

        }

        public async Task<bool> UpdateAsync(UpdateGameDto dto)
        {
            SanitizeInput(dto);

            var genres = new List<Genre>();
            var publishers = new List<Publisher>();

            var duplicate = await _repository.ExistsByNameExceptIdAsync(dto.Name, dto.Id);

            if (duplicate)
                throw new InvalidOperationException($"Game with name {dto.Name} already exists.");

            var gameExist = await _repository.GetByIdAsync(dto.Id)
                ?? throw new NotFoundException($"Game with id {dto.Id} not found.");

            foreach (var genreId in dto.GenreId)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);

                if (genre == null)
                    throw new NotFoundException($"Genre with id {genreId} not found.");
                genres.Add(genre);
            }

            foreach (var publisherId in dto.PublisherId)
            {
                var publisher = await _publisherRepository.GetByIdAsync(publisherId);
                if (publisher == null)
                    throw new NotFoundException($"Publisher with id {publisherId} not found.");
                publishers.Add(publisher);
            }

            gameExist.Update(dto.Name, dto.Price, dto.Description, publishers, genres);

            await _repository.UpdateAsync(gameExist);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty");

            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                throw new NotFoundException($"Game with id {id} not found.");
            
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
    internal static partial class GeneratedRegexes
    {
        [GeneratedRegex(@"\s+")]
        internal static partial Regex WhitespaceRegex();

        [GeneratedRegex(@"\s")]
        internal static partial Regex WhitespaceCharRegex();
    }
}
