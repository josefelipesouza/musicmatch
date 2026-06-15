using MusicMatch.Domain.Entities;

namespace MusicMatch.Domain.Interfaces;

public interface IMensagemRepository
{
    Task<IEnumerable<Mensagem>> GetByUsuarioAsync(Guid usuarioId);
    Task AddAsync(Mensagem mensagem);
}