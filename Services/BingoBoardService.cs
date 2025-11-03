using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using BingoMAUI.Models;

namespace BingoMAUI.Services
{
    public class BingoBoardService : IBingoBoardService
    {
        private readonly string _dataFilePath;
        private readonly JsonSerializerOptions _jsonOptions;

        public BingoBoardService()
        {
            _dataFilePath = Path.Combine(FileSystem.AppDataDirectory, "bingoboards.json");
            _jsonOptions = new JsonSerializerOptions
            {
                WriteIndented = true
            };
        }

        public async Task<List<BingoBoard>> GetAllBoardsAsync()
        {
            if (!File.Exists(_dataFilePath))
            {
                return new List<BingoBoard>();
            }
            var json = await File.ReadAllTextAsync(_dataFilePath);
            var boards = JsonSerializer.Deserialize<List<BingoBoard>>(json, _jsonOptions);
            return boards ?? new List<BingoBoard>();
        }

        public async Task<BingoBoard?> GetBoardByIdAsync(string id)
        {
            var boards = await GetAllBoardsAsync();
            return boards.FirstOrDefault(b => b.Id == id);
        }

        public async Task SaveBoardAsync(BingoBoard board)
        {
            var boards = await GetAllBoardsAsync();

            var existingBoard = boards.FirstOrDefault(b => b.Id == board.Id);
            if (existingBoard != null)
            {
                boards.Remove(existingBoard);
            }
            boards.Add(board);

            var json = JsonSerializer.Serialize(boards, _jsonOptions);
            await File.WriteAllTextAsync(_dataFilePath, json);
        }

        public async Task DeleteBoardAsync(string id)
        {
            var boards = await GetAllBoardsAsync();
            var boardToRemove = boards.FirstOrDefault(b => b.Id == id);
            if (boardToRemove != null)
            {
                boards.Remove(boardToRemove);
                var json = JsonSerializer.Serialize(boards, _jsonOptions);
                await File.WriteAllTextAsync(_dataFilePath, json);
            }
        }
    }
}
