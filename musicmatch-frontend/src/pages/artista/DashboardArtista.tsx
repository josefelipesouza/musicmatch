import { useEffect, useState } from 'react'
import { useAuth } from '../../hooks/useAuth'
import { useLogout } from '../../hooks/useLogout'
import { LocationPicker } from '../../components/location/LocationPicker'
import type { LocationData } from '../../types'
import { api } from '../../services/api'
import { useNavigate } from 'react-router-dom'

interface AgendaItem {
  id: string
  data: string
  horarioInicial: string
  horarioFinal: string
  baseCacheHora: number
  disponivel: boolean
  formatoShow: string
  equipamentoProprio: boolean
  cidade: string
}

interface ArtistaInfo {
  id: string
  nome: string
  email: string
}

const FORMATOS = [
  { value: 1,  label: 'Cantor Solo' },
  { value: 2,  label: 'Duo Vocal' },
  { value: 3,  label: 'Coral Pequeno' },
  { value: 4,  label: 'Coral Grande' },
  { value: 5,  label: 'Banda Rock' },
  { value: 6,  label: 'Banda Samba' },
  { value: 7,  label: 'Banda Forró' },
  { value: 8,  label: 'Banda MPB' },
  { value: 9,  label: 'Banda Funk' },
  { value: 10, label: 'Banda Gospel' },
  { value: 11, label: 'Banda Jazz' },
  { value: 12, label: 'Banda Axé' },
  { value: 13, label: 'Banda Sertanejo' },
  { value: 14, label: 'Guitarrista' },
  { value: 15, label: 'Baixista' },
  { value: 16, label: 'Violonista' },
  { value: 17, label: 'Violinista' },
  { value: 18, label: 'Violista' },
  { value: 19, label: 'Violoncelista' },
  { value: 20, label: 'Harpista' },
  { value: 21, label: 'Cavaquinhista' },
  { value: 22, label: 'Bandolinista' },
  { value: 23, label: 'Tecladista' },
  { value: 24, label: 'Pianista' },
  { value: 25, label: 'Organista' },
  { value: 26, label: 'Acordeonista' },
  { value: 27, label: 'Baterista' },
  { value: 28, label: 'Percussionista' },
  { value: 29, label: 'Pandeirista' },
  { value: 30, label: 'Saxofonista' },
  { value: 31, label: 'Trompetista' },
  { value: 32, label: 'Trombonista' },
  { value: 33, label: 'Flautista' },
  { value: 34, label: 'Clarinetista' },
  { value: 35, label: 'DJ' },
  { value: 36, label: 'Produção Musical' },
  { value: 37, label: 'Outro' },
]

const AGENDA_VAZIA = {
  data: '',
  horarioInicial: '',
  horarioFinal: '',
  baseCacheHora: '',
  formatoShow: '',
  equipamentoProprio: false,
}

export function DashboardArtista() {
  const navigate = useNavigate()
  const logout = useLogout()
  const { userId, token } = useAuth()
  const [artista, setArtista] = useState<ArtistaInfo | null>(null)
  const [agendas, setAgendas] = useState<AgendaItem[]>([])
  const [showModal, setShowModal] = useState(false)
  const [loading, setLoading] = useState(true)
  const [salvando, setSalvando] = useState(false)
  const [localizacao, setLocalizacao] = useState<LocationData | null>(null)
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [novaAgenda, setNovaAgenda] = useState(AGENDA_VAZIA)

  const hoje = new Date().toISOString().split('T')[0]

  const headers = { Authorization: `Bearer ${token}` }

  useEffect(() => {
    if (!userId) return
    Promise.all([
      api.get(`/api/artistas/${userId}`, { headers }),
      api.get(`/api/artistas/${userId}/agendas`, { headers }),
    ]).then(([artistaRes, agendasRes]) => {
      setArtista(artistaRes.data)
      setAgendas(agendasRes.data)
    }).catch(err => console.error('Erro ao carregar dados:', err))
    .finally(() => setLoading(false))
}, [userId, token])

  const podeCancel = (agenda: AgendaItem) => {
    const dataAgenda = new Date(`${agenda.data.split('T')[0]}T${agenda.horarioInicial}`)
    return dataAgenda > new Date() && agenda.disponivel
  }

  const cancelar = async (id: string) => {
    await api.delete(`/api/artistas/agenda/${id}`, { headers })
    setAgendas(prev => prev.map(a => a.id === id ? { ...a, disponivel: false } : a))
  }

  const validate = () => {
    const e: Record<string, string> = {}
    if (!novaAgenda.data) e.data = 'Selecione uma data'
    if (!novaAgenda.horarioInicial) e.horarioInicial = 'Informe o horário inicial'
    if (!novaAgenda.horarioFinal) e.horarioFinal = 'Informe o horário final'
    if (novaAgenda.horarioFinal <= novaAgenda.horarioInicial) e.horarioFinal = 'Horário final deve ser após o inicial'
    if (!novaAgenda.baseCacheHora) e.baseCacheHora = 'Informe o cache por hora'
    if (!novaAgenda.formatoShow) e.formatoShow = 'Selecione o formato do show'
    if (!localizacao || localizacao.latitude === 0) e.localizacao = 'Selecione uma localização'
    setErrors(e)
    return Object.keys(e).length === 0
  }

  const criarAgenda = async () => {
    if (!userId || !validate() || !localizacao) return
    setSalvando(true)
    try {
      await api.post('/api/artistas/agenda', {
        artistaId: userId,
        formatoShow: parseInt(novaAgenda.formatoShow),
        equipamentoProprio: novaAgenda.equipamentoProprio,
        data: new Date(novaAgenda.data + 'T00:00:00').toISOString(),
        horarioInicial: novaAgenda.horarioInicial + ':00',
        horarioFinal: novaAgenda.horarioFinal + ':00',
        baseCacheHora: parseFloat(novaAgenda.baseCacheHora),
        cidade: localizacao.cidade,
        latitude: localizacao.latitude,
        longitude: localizacao.longitude,
      }, { headers })

      const res = await api.get(`/api/artistas/${userId}/agendas`, { headers })
      setAgendas(res.data)
      setShowModal(false)
      setNovaAgenda(AGENDA_VAZIA)
      setLocalizacao(null)
      setErrors({})
    } catch (err) {
      console.error(err)
    } finally {
      setSalvando(false)
    }
  }

  const fecharModal = () => {
    setShowModal(false)
    setNovaAgenda(AGENDA_VAZIA)
    setLocalizacao(null)
    setErrors({})
  }

  if (loading) return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-50 to-blue-50">
      <p className="text-gray-400 text-sm">Carregando...</p>
    </div>
  )

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-50">
      {/* Header */}
      <div className="bg-white/80 backdrop-blur-sm border-b border-gray-200 px-6 py-5 sticky top-0 z-20 shadow-sm">
        <div className="max-w-3xl mx-auto flex items-center justify-between gap-4">
          <div className="flex items-center gap-3">
            <div className="w-11 h-11 rounded-xl bg-gradient-to-br from-blue-600 to-indigo-600 flex items-center justify-center text-white font-bold text-lg shadow-md shrink-0">
              {artista?.nome?.charAt(0).toUpperCase() ?? '♪'}
            </div>
            <div>
              <h1 className="text-xl font-semibold text-gray-800">{artista?.nome}</h1>
              <p className="text-sm text-gray-500 mt-0.5">{artista?.email}</p>
            </div>
          </div>
          <div className="flex gap-2 shrink-0">
            <button onClick={() => navigate('/perfil/artista')} className="px-4 py-2 text-sm border border-gray-300 rounded-lg text-gray-600 hover:bg-gray-50 transition-colors">
              Editar perfil
            </button>
            <button onClick={logout} className="px-4 py-2 text-sm border border-red-200 rounded-lg text-red-500 hover:bg-red-50 transition-colors">
              Sair
            </button>
          </div>
        </div>
      </div>

      {/* Conteúdo */}
      <div className="max-w-3xl mx-auto px-6 py-8 flex flex-col gap-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-xl font-bold text-gray-800">Meus agendamentos</h2>
            <p className="text-sm text-gray-500 mt-0.5">
              {agendas.filter(a => a.disponivel).length} agendamento{agendas.filter(a => a.disponivel).length !== 1 && 's'} ativo{agendas.filter(a => a.disponivel).length !== 1 && 's'}
            </p>
          </div>
          <button
            onClick={() => setShowModal(true)}
            className="px-5 py-2.5 bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 text-white text-sm font-medium rounded-xl transition-all shadow-md hover:shadow-lg"
          >
            + Novo agendamento
          </button>
        </div>

        {/* Lista */}
        <div className="flex flex-col gap-3 max-h-[65vh] overflow-y-auto pr-1">
          {agendas.length === 0 && (
            <div className="bg-white rounded-2xl border border-gray-200 p-10 text-center shadow-sm">
              <div className="text-4xl mb-2">🎵</div>
              <p className="text-gray-400 text-sm">Nenhum agendamento ainda.</p>
              <p className="text-gray-300 text-xs mt-1">Clique em "Novo agendamento" para começar.</p>
            </div>
          )}
          {agendas.map((agenda) => (
            <div
              key={agenda.id}
              className={`bg-white rounded-2xl border p-5 flex items-center justify-between gap-4 shadow-sm hover:shadow-md transition-shadow ${
                !agenda.disponivel ? 'opacity-50 border-gray-100' : 'border-gray-200'
              }`}
            >
              <div className="flex flex-col gap-1.5">
                <p className="font-semibold text-gray-800 text-sm capitalize">
                  {new Date(agenda.data).toLocaleDateString('pt-BR', {
                    weekday: 'long', day: '2-digit', month: 'long', year: 'numeric'
                  })}
                </p>
                <p className="text-xs text-gray-500">
                  🕒 {agenda.horarioInicial.substring(0, 5)} → {agenda.horarioFinal.substring(0, 5)}
                </p>
                <p className="text-xs text-gray-500">
                  📍 {agenda.cidade}
                </p>
                <div className="flex items-center gap-2 mt-1 flex-wrap">
                  <span className="text-xs font-semibold uppercase tracking-wider text-blue-600 bg-blue-50 px-2 py-0.5 rounded-full">
                    {agenda.formatoShow}
                  </span>
                  <span className="text-xs font-medium text-emerald-600 bg-emerald-50 px-2 py-0.5 rounded-full">
                    R$ {agenda.baseCacheHora}/hora
                  </span>
                  {agenda.equipamentoProprio && (
                    <span className="text-xs text-purple-600 bg-purple-50 px-2 py-0.5 rounded-full">
                      🎸 Equipamento próprio
                    </span>
                  )}
                </div>
              </div>
              <div className="flex items-center gap-2 shrink-0">
                {!agenda.disponivel && (
                  <span className="text-xs text-gray-400 bg-gray-100 px-3 py-1.5 rounded-full font-medium">
                    Cancelado
                  </span>
                )}
                {podeCancel(agenda) && (
                  <button
                    onClick={() => cancelar(agenda.id)}
                    className="text-xs text-red-500 hover:text-white hover:bg-red-500 border border-red-200 hover:border-red-500 px-3 py-1.5 rounded-lg transition-colors font-medium"
                  >
                    Cancelar
                  </button>
                )}
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Modal */}
      {showModal && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 px-4">
          <div className="bg-white rounded-2xl shadow-2xl p-6 w-full max-w-md flex flex-col gap-4 max-h-[90vh] overflow-y-auto">
            <div>
              <h3 className="text-lg font-bold text-gray-800">Novo agendamento</h3>
              <p className="text-sm text-gray-500 mt-0.5">Preencha os dados do seu show disponível</p>
            </div>

            {/* Formato do show */}
            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Formato do show</label>
              <select
                value={novaAgenda.formatoShow}
                onChange={(e) => setNovaAgenda({ ...novaAgenda, formatoShow: e.target.value })}
                className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100 bg-white"
              >
                <option value="">Selecione...</option>
                {FORMATOS.map(f => (
                  <option key={f.value} value={f.value}>{f.label}</option>
                ))}
              </select>
              {errors.formatoShow && <p className="text-xs text-red-500">{errors.formatoShow}</p>}
            </div>

            {/* Localização */}
            <LocationPicker
              label="Local do show"
              placeholder="Ex: Belo Horizonte, São Paulo..."
              value={localizacao}
              onChange={setLocalizacao}
              error={errors.localizacao}
            />

            {/* Data */}
            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Data</label>
              <input
                type="date"
                min={hoje}
                value={novaAgenda.data}
                onChange={(e) => setNovaAgenda({ ...novaAgenda, data: e.target.value })}
                className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              />
              {errors.data && <p className="text-xs text-red-500">{errors.data}</p>}
            </div>

            {/* Horários */}
            <div className="flex gap-3">
              <div className="flex flex-col gap-1 flex-1">
                <label className="text-sm font-medium text-gray-700">Horário inicial</label>
                <input
                  type="time"
                  value={novaAgenda.horarioInicial}
                  onChange={(e) => setNovaAgenda({ ...novaAgenda, horarioInicial: e.target.value })}
                  className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                />
                {errors.horarioInicial && <p className="text-xs text-red-500">{errors.horarioInicial}</p>}
              </div>
              <div className="flex flex-col gap-1 flex-1">
                <label className="text-sm font-medium text-gray-700">Horário final</label>
                <input
                  type="time"
                  value={novaAgenda.horarioFinal}
                  onChange={(e) => setNovaAgenda({ ...novaAgenda, horarioFinal: e.target.value })}
                  className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                />
                {errors.horarioFinal && <p className="text-xs text-red-500">{errors.horarioFinal}</p>}
              </div>
            </div>

            {/* Cache */}
            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Cache por hora (R$)</label>
              <input
                type="number"
                min="0"
                value={novaAgenda.baseCacheHora}
                onChange={(e) => setNovaAgenda({ ...novaAgenda, baseCacheHora: e.target.value })}
                className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                placeholder="Ex: 150.00"
              />
              {errors.baseCacheHora && <p className="text-xs text-red-500">{errors.baseCacheHora}</p>}
            </div>

            {/* Equipamento */}
            <div className="flex items-center gap-3">
              <input
                type="checkbox"
                id="equipamento"
                checked={novaAgenda.equipamentoProprio}
                onChange={(e) => setNovaAgenda({ ...novaAgenda, equipamentoProprio: e.target.checked })}
                className="w-4 h-4 accent-blue-600"
              />
              <label htmlFor="equipamento" className="text-sm text-gray-700">
                Possuo equipamento próprio (som, instrumentos, etc.)
              </label>
            </div>

            {/* Botões */}
            <div className="flex gap-3 mt-2">
              <button
                onClick={fecharModal}
                className="flex-1 py-2.5 border border-gray-300 text-gray-600 text-sm font-medium rounded-xl hover:bg-gray-50 transition-colors"
              >
                Cancelar
              </button>
              <button
                onClick={criarAgenda}
                disabled={salvando}
                className="flex-1 py-2.5 bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 disabled:opacity-50 text-white text-sm font-medium rounded-xl transition-all shadow-md"
              >
                {salvando ? 'Salvando...' : 'Criar'}
              </button>
            </div>
          </div>
        </div>
      )}
    </div>
  )
}
