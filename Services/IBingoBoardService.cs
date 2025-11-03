using System.Collections.Generic;
using System.Threading.Tasks;
using BingoMAUI.Models;

namespace BingoMAUI.Services
{
    public interface IBingoBoardService
    {
        Task<List<BingoBoard>> GetAllBoardsAsync();
        Task<BingoBoard?> GetBoardByIdAsync(string id);
        Task SaveBoardAsync(BingoBoard board);
        Task DeleteBoardAsync(string id);
    }
}
