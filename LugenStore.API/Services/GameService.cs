using LugenStore.API.Common.Validation;
using LugenStore.API.DTOs.Game;
using LugenStore.API.Exceptions;
using LugenStore.API.Models;
using LugenStore.API.Repositories.Interfaces;
using LugenStore.API.Services.Interfaces;
using System.Text.RegularExpressions;

namespace LugenStore.API.Services
{
    public class GameService : IGameService
    {
        private readonly IGameRepository _repository;
        private readonly IGenreRepository _genreRepository;
        private readonly IPublisherRepository _publisherRepository;

        public GameService(IGameRepository repository, IGenreRepository genreRepository, IPublisherRepository publisherRepository)
        {
            _repository = repository;
            _genreRepository = genreRepository;
            _publisherRepository = publisherRepository;
        }

        private async Task ValidateGame(GameBaseDto dto)
        { 
            dto.Name = dto.Name.Trim();
            dto.Description = dto.Description.Trim();

            dto.Name = Regex.Replace(dto.Name, @"\s+", " ");
            dto.Description = Regex.Replace(dto.Description, @"\s+", " ");

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
                Description = game.Description
            };
        }

        public async Task<GameResponseDto> CreateAsync(CreateGameDto dto)
        {
            await ValidateGame(dto);

            var genres = new List<Genre>();

            foreach (var genreId in dto.GenreId)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);

                if (genre == null)
                    throw new NotFoundException($"Genre with id {genreId} not found.");
                genres.Add(genre);
            }

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

            await ValidateGame(dto);

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

            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                throw new NotFoundException($"Game with id {id} not found.");

            return true;
        }
    }
}
