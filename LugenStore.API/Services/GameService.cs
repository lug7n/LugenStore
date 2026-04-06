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

        public GameService(IGameRepository repository, IGenreRepository genreRepository)
        {
            _repository = repository;
            _genreRepository = genreRepository;
        }

        private async Task ValidateGame(GameBaseDto dto)
        {
            if (dto is null)
                throw new ValidationException("Game is required.");

            dto.Name = dto.Name.Trim();
            dto.Name = Regex.Replace(dto.Name, @"\s+", " ");

            if (dto.Price < 0)
                throw new ValidationException("Price cannot be negative.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new ValidationException("Game name is required.");

            if (string.IsNullOrWhiteSpace(dto.Description))
                throw new ValidationException("Game description cannot be null or empty");

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

            if (dto.GenreId is null || !dto.GenreId.Any())
                throw new ValidationException("GenreId cannot be empty.");

            if (dto.GenreId.Any(g => g == Guid.Empty))
                throw new ValidationException("GenreId cannot be empty.");

            if (dto.PublisherId == Guid.Empty)
                throw new ValidationException("PublisherId cannot be empty.");

            var genres = new List<Genre>();

            foreach (var genreId in dto.GenreId)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);

                if (genre == null)
                    throw new NotFoundException($"Genre with id {genreId} not found.");
                genres.Add(genre);
            }

            //if (!await _publisherRepository.ExistsByIdAsync(dto.PublisherId))
            //    throw new NotFoundException($"Publisher with id {dto.PublisherId} not found.");

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

            if (dto.GenreId is null || !dto.GenreId.Any())
                throw new ValidationException("GenreId cannot be empty.");

            if (dto.GenreId.Any(g => g == Guid.Empty))
                throw new ValidationException("GenreId cannot be empty.");

            if (dto.PublisherId == Guid.Empty)
                throw new ValidationException("PublisherId cannot be empty.");

            var gameExist = await _repository.GetByIdAsync(dto.Id)
                ?? throw new NotFoundException($"Game with id {dto.Id} not found.");

            foreach (var genreId in dto.GenreId)
            {
                var genre = await _genreRepository.GetByIdAsync(genreId);

                if (genre == null)
                    throw new NotFoundException($"Genre with id {genreId} not found.");
                genres.Add(genre);
            }

            //if (!await _publisherRepository.ExistsByIdAsync(dto.PublisherId))
            //    throw new NotFoundException($"Publisher with id {dto.PublisherId} not found.");

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
                throw new ValidationException($"Id cannot be empty");

            var deleted = await _repository.DeleteAsync(id);

            if (!deleted)
                throw new NotFoundException($"Game with id {id} not found.");

            return true;
        }
    }
}
