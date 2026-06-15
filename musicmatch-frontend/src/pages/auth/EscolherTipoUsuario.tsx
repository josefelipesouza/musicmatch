import { useNavigate, useLocation } from 'react-router-dom'

export function EscolherTipoUsuario() {
  const navigate = useNavigate()
  const location = useLocation()

  // Dados vindos do Google Auth (primeiroAcesso: true)
  const state = location.state as { email?: string; nome?: string } | null

  const handleEscolha = (tipo: 'artista' | 'contratante') => {
    const destino = tipo === 'artista' ? '/cadastro/artista' : '/cadastro/contratante'
    navigate(destino, { state })
  }

  return (
    <div className="min-h-screen bg-gray-50 flex items-center justify-center px-4">
      <div className="max-w-lg w-full bg-white rounded-2xl shadow p-8 flex flex-col gap-8">
        {/* Header */}
        <div className="text-center">
          <div className="text-4xl mb-3">🎵</div>
          <h1 className="text-2xl font-semibold text-gray-800">Bem-vindo ao MusicMatch!</h1>
          {state?.nome && (
            <p className="text-gray-500 mt-1 text-sm">Olá, {state.nome.split(' ')[0]}! Como você vai usar a plataforma?</p>
          )}
          {!state?.nome && (
            <p className="text-gray-500 mt-1 text-sm">Como você vai usar a plataforma?</p>
          )}
        </div>

        {/* Opções */}
        <div className="flex flex-col gap-4">
          {/* Artista */}
          <button
            type="button"
            onClick={() => handleEscolha('artista')}
            className="group flex items-center gap-5 p-5 rounded-xl border-2 border-gray-200 hover:border-blue-500 hover:bg-blue-50 transition-all text-left"
          >
            <div className="w-14 h-14 rounded-xl bg-blue-100 group-hover:bg-blue-200 flex items-center justify-center text-2xl transition-colors flex-shrink-0">
              🎸
            </div>
            <div>
              <p className="font-semibold text-gray-800 text-base">Sou músico / artista</p>
              <p className="text-sm text-gray-500 mt-0.5">
                Quero criar meu perfil e receber propostas de shows e eventos
              </p>
            </div>
            <div className="ml-auto text-gray-300 group-hover:text-blue-400 transition-colors text-xl">→</div>
          </button>

          {/* Contratante */}
          <button
            type="button"
            onClick={() => handleEscolha('contratante')}
            className="group flex items-center gap-5 p-5 rounded-xl border-2 border-gray-200 hover:border-purple-500 hover:bg-purple-50 transition-all text-left"
          >
            <div className="w-14 h-14 rounded-xl bg-purple-100 group-hover:bg-purple-200 flex items-center justify-center text-2xl transition-colors flex-shrink-0">
              🎪
            </div>
            <div>
              <p className="font-semibold text-gray-800 text-base">Sou contratante</p>
              <p className="text-sm text-gray-500 mt-0.5">
                Quero criar eventos e encontrar artistas para minha festa ou estabelecimento
              </p>
            </div>
            <div className="ml-auto text-gray-300 group-hover:text-purple-400 transition-colors text-xl">→</div>
          </button>
        </div>

        {state?.email && (
          <p className="text-center text-xs text-gray-400">
            Conectado com <span className="font-medium">{state.email}</span>
          </p>
        )}
      </div>
    </div>
  )
}
