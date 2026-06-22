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
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-50 flex items-center justify-center px-4 py-10">
      <div className="max-w-lg w-full bg-white rounded-2xl shadow-2xl p-8 flex flex-col gap-8">
        {/* Header */}
        <div className="text-center">
          <div className="w-16 h-16 mx-auto rounded-xl bg-gradient-to-br from-blue-600 to-indigo-600 flex items-center justify-center text-3xl shadow-md mb-4">
            🎵
          </div>
          <h1 className="text-2xl font-bold text-gray-800">Bem-vindo ao MusicMatch!</h1>
          {state?.nome ? (
            <p className="text-gray-500 mt-1 text-sm">
              Olá, {state.nome.split(' ')[0]}! Como você vai usar a plataforma?
            </p>
          ) : (
            <p className="text-gray-500 mt-1 text-sm">Como você vai usar a plataforma?</p>
          )}
        </div>

        {/* Opções */}
        <div className="flex flex-col gap-4">
          {/* Artista */}
          <button
            type="button"
            onClick={() => handleEscolha('artista')}
            className="group flex items-center gap-5 p-5 rounded-2xl border border-gray-200 hover:border-blue-400 hover:shadow-lg bg-white transition-all text-left"
          >
            <div className="w-14 h-14 rounded-xl bg-gradient-to-br from-blue-500 to-indigo-600 flex items-center justify-center text-2xl shadow-sm flex-shrink-0 transition-transform group-hover:scale-105">
              🎸
            </div>
            <div className="flex-1">
              <span className="text-xs font-semibold uppercase tracking-wider text-blue-600 bg-blue-50 px-2 py-0.5 rounded-full w-max">
                Artista
              </span>
              <p className="font-semibold text-gray-800 text-base mt-1">Sou músico / artista</p>
              <p className="text-sm text-gray-500 mt-0.5">
                Quero criar meu perfil e receber propostas de shows e eventos
              </p>
            </div>
            <div className="text-gray-300 group-hover:text-blue-500 group-hover:translate-x-0.5 transition-all text-xl">→</div>
          </button>

          {/* Contratante */}
          <button
            type="button"
            onClick={() => handleEscolha('contratante')}
            className="group flex items-center gap-5 p-5 rounded-2xl border border-gray-200 hover:border-purple-400 hover:shadow-lg bg-white transition-all text-left"
          >
            <div className="w-14 h-14 rounded-xl bg-gradient-to-br from-purple-500 to-pink-600 flex items-center justify-center text-2xl shadow-sm flex-shrink-0 transition-transform group-hover:scale-105">
              🎪
            </div>
            <div className="flex-1">
              <span className="text-xs font-semibold uppercase tracking-wider text-purple-600 bg-purple-50 px-2 py-0.5 rounded-full w-max">
                Contratante
              </span>
              <p className="font-semibold text-gray-800 text-base mt-1">Sou contratante</p>
              <p className="text-sm text-gray-500 mt-0.5">
                Quero criar eventos e encontrar artistas para minha festa ou estabelecimento
              </p>
            </div>
            <div className="text-gray-300 group-hover:text-purple-500 group-hover:translate-x-0.5 transition-all text-xl">→</div>
          </button>
        </div>

        {state?.email && (
          <p className="text-center text-xs text-gray-400">
            Conectado com <span className="font-medium text-gray-500">{state.email}</span>
          </p>
        )}
      </div>
    </div>
  )
}
