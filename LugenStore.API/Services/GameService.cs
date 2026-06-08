using LugenStore.API.Common.Validation;
using LugenStore.API.DTOs.Game;
using LugenStore.API.Exceptions;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using LugenStore.API.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LugenStore.API.Services
{
    public partial class GameService(IGameRepository _repository, IGenreRepository _genreRepository, IPublisherRepository _publisherRepository) : IGameService
    {
        private static void ValidateGame(GameBaseDto dto)
        { 
            dto.Name = dto.Name.Trim();
            dto.Description = dto.Description.Trim();

            dto.Name = GeneratedRegexes.WhitespaceRegex().Replace(dto.Name, " ");
            dto.Description = GeneratedRegexes.WhitespaceRegex().Replace(dto.Description, " ");

            if (dto.Price < 0)
                throw new ValidationException("Price cannot be negative.");

            if (!ValidationPatterns.NameRegex.IsMatch(dto.Name))
                throw new ValidationException("Game name can only contain letters, numbers, spaces, and basic punctuation.");
        }

        public async Task<IEnumerable<GameResponseDto>> GetAllAsync()
        {
            var games = await _repository.GetAllAsync();

            return games.Select(game => new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price,
                Publisher = game.Publisher.Name,
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
                Publisher = game.Publisher.Name,
                Genres = game.Genres.Select(g => g.Name).ToList(),
                Description = game.Description,
                CreatedAt = game.CreatedAt
            };
        }

        public async Task<GameResponseDto> CreateAsync(CreateGameDto dto)
        {
            ValidateGame(dto);

            var genres = new List<Genre>();

            if (dto.GenreId == null || dto.GenreId.Count == 0)
                throw new ValidationException("At least one genre must be provided.");

            foreach (var genreId in dto.GenreId)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);
                if (genre == null)
                    throw new NotFoundException($"Genre with id {genreId} not found.");
                genres.Add(genre);
            }

            if (await _repository.ExistsByNameAsync(dto.Name))
                throw new InvalidOperationException($"Game with name {dto.Name} already exists.");

            if (!await _publisherRepository.ExistsByIdAsync(dto.PublisherId))
                throw new NotFoundException($"publisher with id {dto.PublisherId} not found.");

            var game = new Game
            {
                Name = dto.Name,
                Price = dto.Price,
                PublisherId = dto.PublisherId,
                Genres = genres,
                Description = dto.Description,
                CreatedAt = DateTime.UtcNow
            };

            await _repository.CreateAsync(game);

            var createdGame = await _repository.GetByIdAsync(game.Id)
                ?? throw new NotFoundException($"Game with id {game.Id} not found after creation."); // This should never happen, but we want to be sure.

            return new GameResponseDto
            {
                Id = game.Id,
                Name = game.Name,
                Price = game.Price,
                Description = game.Description,
                Publisher = createdGame.Publisher.Name,
                Genres = createdGame.Genres.Select(g => g.Name).ToList(),
                CreatedAt = game.CreatedAt,

            };
        }

        public async Task<bool> UpdateAsync(UpdateGameDto dto)
        {
            var genres = new List<Genre>();

            ValidateGame(dto);

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

            if (!await _publisherRepository.ExistsByIdAsync(dto.PublisherId))
                throw new NotFoundException($"Publisher with id {dto.PublisherId} not found.");

            var game = new Game
            {
                Id = dto.Id,
                Name = dto.Name,
                Price = dto.Price,
                PublisherId = dto.PublisherId,
                Genres = genres,
                Description = dto.Description,
                CreatedAt = gameExist.CreatedAt
            };

            await _repository.UpdateAsync(game);

            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
                throw new ValidationException("Id cannot be empty");

            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                throw new NotFoundException($"Game with id {id} not found.");

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
