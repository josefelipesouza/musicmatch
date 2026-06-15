import { useEffect, useState } from 'react'
import { useAuth } from '../../hooks/useAuth'
import { useLogout } from '../../hooks/useLogout'
import { LocationPicker } from '../../components/location/LocationPicker'
import type { LocationData } from '../../types'
import { api } from '../../services/api'
import { useNavigate } from 'react-router-dom'

interface EventoItem {
  id: string
  localizacao: string
  latitude: number
  longitude: number
  raioKm: number
  formatoShow: number
  tipo: number
  dataInicio: string
  dataFim: string
  horarioInicio: string
  horarioFim: string
  equipamentoProprio: boolean
  baseCacheHoraAte: number
}

interface ContratanteInfo {
  id: string
  nome: string
  email: string
}

interface MatchItem {
  agendaId: string
  artistaId: string
  nome: string
  razaoSocial?: string
  cidade: string
  distanciaKm: number
  equipamentoProprio: boolean
  formatosShow: string[]
  baseCacheHora: number
  celular1: string
  celular2?: string
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

const TIPOS_EVENTO = [
  { value: 1, label: 'Show' },
  { value: 2, label: 'Festa' },
  { value: 3, label: 'Casamento' },
  { value: 4, label: 'Corporativo' },
  { value: 5, label: 'Festival' },
  { value: 6, label: 'Aniversário' },
  { value: 7, label: 'Formatura' },
  { value: 8, label: 'Outro' },
]

const EVENTO_VAZIO = {
  tipo: '',
  formatoShow: '',
  dataInicio: '',
  dataFim: '',
  horarioInicio: '',
  horarioFim: '',
  baseCacheHoraAte: '',
  raioKm: '15',
  equipamentoProprio: false,
}

const getLabelByValue = (array: { value: number; label: string }[], value: number) =>
  array.find(item => item.value === value)?.label || 'Não especificado'

export function DashboardContratante() {

  const navigate = useNavigate()
  const logout = useLogout()
  const { userId, token } = useAuth()
  const [contratante, setContratante] = useState<ContratanteInfo | null>(null)
  const [eventos, setEventos] = useState<EventoItem[]>([])
  const [showModal, setShowModal] = useState(false)
  const [loading, setLoading] = useState(true)
  const [salvando, setSalvando] = useState(false)
  const [localizacao, setLocalizacao] = useState<LocationData | null>(null)
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [novoEvento, setNovoEvento] = useState(EVENTO_VAZIO)

  // Match
  const [showMatchModal, setShowMatchModal] = useState(false)
  const [matchResultados, setMatchResultados] = useState<MatchItem[]>([])
  const [loadingMatch, setLoadingMatch] = useState(false)
  const [eventoSelecionado, setEventoSelecionado] = useState<EventoItem | null>(null)

  const hoje = new Date().toISOString().split('T')[0]
  const headers = { Authorization: `Bearer ${token}` }

  const [notificando, setNotificando] = useState<string | null>(null)
  const [notificadosPorEvento, setNotificadosPorEvento] = useState<Record<string, string[]>>(() => {
    try {
      const salvo = localStorage.getItem(`notificados_${userId}`)
      return salvo ? JSON.parse(salvo) : {}
    } catch {
      return {}
    }
  })

  const cancelarEvento = async (eventoId: string) => {
    if (!confirm('Tem certeza que deseja cancelar este evento?')) return
    try {
      await api.delete(`/api/eventos/${eventoId}`, { headers })
      setEventos(prev => prev.filter(e => e.id !== eventoId))
    } catch (err) {
      console.error(err)
      alert('Erro ao cancelar evento.')
    }
  }

  const jaNotificado = (agendaId: string) =>
    eventoSelecionado
      ? (notificadosPorEvento[eventoSelecionado.id]?.includes(agendaId) ?? false)
      : false

  useEffect(() => {
    if (!userId) return
    Promise.all([
      api.get(`/api/contratantes/${userId}`, { headers }),
      api.get(`/api/contratantes/${userId}/eventos`, { headers }),
    ]).then(([contratanteRes, eventosRes]) => {
      setContratante(contratanteRes.data)
      setEventos(eventosRes.data)
    }).catch(err => console.error(err))
      .finally(() => setLoading(false))
  }, [userId])

  const validate = () => {
    const e: Record<string, string> = {}
    if (!novoEvento.tipo) e.tipo = 'Selecione o tipo do evento'
    if (!novoEvento.formatoShow) e.formatoShow = 'Selecione o formato de show desejado'
    if (!novoEvento.dataInicio) e.dataInicio = 'Informe a data de início'
    if (!novoEvento.dataFim) e.dataFim = 'Informe a data de término'
    if (novoEvento.dataFim < novoEvento.dataInicio) e.dataFim = 'Data fim não pode ser anterior à data início'
    if (!novoEvento.horarioInicio) e.horarioInicio = 'Informe o horário inicial'
    if (!novoEvento.horarioFim) e.horarioFim = 'Informe o horário final'
    if (novoEvento.dataInicio === novoEvento.dataFim && novoEvento.horarioFim <= novoEvento.horarioInicio)
      e.horarioFim = 'Horário final deve ser após o inicial no mesmo dia'
    if (!novoEvento.baseCacheHoraAte) e.baseCacheHoraAte = 'Informe o limite máximo de cache'
    if (!novoEvento.raioKm || parseFloat(novoEvento.raioKm) <= 0) e.raioKm = 'O raio de busca deve ser maior que zero'
    if (!localizacao || localizacao.latitude === 0) e.localizacao = 'Selecione uma localização'
    setErrors(e)
    return Object.keys(e).length === 0
  }

  const criarEvento = async () => {
    if (!userId || !validate() || !localizacao) return
    setSalvando(true)
    try {
      await api.post('/api/contratantes/evento', {
        contratanteId: userId,
        localizacao: localizacao.cidade,
        latitude: localizacao.latitude,
        longitude: localizacao.longitude,
        raioKm: parseFloat(novoEvento.raioKm),
        formatoShow: parseInt(novoEvento.formatoShow),
        tipo: parseInt(novoEvento.tipo),
        dataInicio: new Date(novoEvento.dataInicio + 'T00:00:00').toISOString(),
        dataFim: new Date(novoEvento.dataFim + 'T00:00:00').toISOString(),
        horarioInicio: novoEvento.horarioInicio + ':00',
        horarioFim: novoEvento.horarioFim + ':00',
        equipamentoProprio: novoEvento.equipamentoProprio,
        baseCacheHoraAte: parseFloat(novoEvento.baseCacheHoraAte),
      }, { headers })

      const res = await api.get(`/api/contratantes/${userId}/eventos`, { headers })
      setEventos(res.data)
      fecharModal()
    } catch (err) {
      console.error(err)
    } finally {
      setSalvando(false)
    }
  }

  const fecharModal = () => {
    setShowModal(false)
    setNovoEvento(EVENTO_VAZIO)
    setLocalizacao(null)
    setErrors({})
  }

  const buscarMatch = async (evento: EventoItem) => {
    setEventoSelecionado(evento)
    setShowMatchModal(true)
    setLoadingMatch(true)
    setMatchResultados([])
    try {
      const res = await api.get('/api/artistas/buscar', {
        headers,
        params: {
          formatoShow: evento.formatoShow,
          latitude: evento.latitude,
          longitude: evento.longitude,
          raioKm: evento.raioKm,
          data: new Date(evento.dataInicio).toISOString(),
          horarioInicio: evento.horarioInicio,
          horarioFim: evento.horarioFim,
          equipamentoProprio: evento.equipamentoProprio || undefined,
          baseCacheHoraAte: evento.baseCacheHoraAte || undefined,
        }
      })
      setMatchResultados(res.data)
    } catch (err) {
      console.error(err)
    } finally {
      setLoadingMatch(false)
    }
  }

  const notificar = async (artistaId: string, agendaId: string, eventoId: string) => {
    setNotificando(agendaId)
    try {
      await api.post('/api/contratantes/notificar-artista', {
        artistaId,
        eventoId,
        contratanteId: userId,
      }, { headers })

      setNotificadosPorEvento(prev => {
        const atuais = prev[eventoId] ?? []
        if (atuais.includes(agendaId)) return prev
        const atualizado = { ...prev, [eventoId]: [...atuais, agendaId] }
        localStorage.setItem(`notificados_${userId}`, JSON.stringify(atualizado))
        return atualizado
      })
    } catch (err) {
      console.error(err)
      alert('Erro ao enviar notificação.')
    } finally {
      setNotificando(null)
    }
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
              {contratante?.nome?.charAt(0).toUpperCase() ?? '♪'}
            </div>
            <div>
              <h1 className="text-xl font-semibold text-gray-800">{contratante?.nome}</h1>
              <p className="text-sm text-gray-500 mt-0.5">{contratante?.email} · Contratante</p>
            </div>
          </div>
          <div className="flex gap-2 shrink-0">
            <button onClick={() => navigate('/perfil/contratante')} className="px-4 py-2 text-sm border border-gray-300 rounded-lg text-gray-600 hover:bg-gray-50 transition-colors">
              Editar perfil
            </button>
            <button
              onClick={logout}
              className="px-4 py-2 text-sm border border-red-200 rounded-lg text-red-500 hover:bg-red-50 transition-colors"
            >
              Sair
            </button>
          </div>
        </div>
      </div>

      {/* Conteúdo */}
      <div className="max-w-3xl mx-auto px-6 py-8 flex flex-col gap-6">
        <div className="flex items-center justify-between">
          <div>
            <h2 className="text-xl font-bold text-gray-800">Meus Eventos</h2>
            <p className="text-sm text-gray-500 mt-0.5">
              {eventos.length} evento{eventos.length !== 1 && 's'} cadastrado{eventos.length !== 1 && 's'}
            </p>
          </div>
          <button
            onClick={() => setShowModal(true)}
            className="px-5 py-2.5 bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 text-white text-sm font-medium rounded-xl transition-all shadow-md hover:shadow-lg"
          >
            + Novo Evento
          </button>
        </div>

        {/* Lista de Eventos */}
        <div className="flex flex-col gap-3 max-h-[65vh] overflow-y-auto pr-1">
          {eventos.length === 0 && (
            <div className="bg-white rounded-2xl border border-gray-200 p-10 text-center shadow-sm">
              <div className="text-4xl mb-2">🎤</div>
              <p className="text-gray-400 text-sm">Nenhum evento criado ainda.</p>
              <p className="text-gray-300 text-xs mt-1">Clique em "Novo Evento" para começar.</p>
            </div>
          )}
          {eventos.map((evento) => (
            <div key={evento.id} className="bg-white rounded-2xl border border-gray-200 p-5 flex items-center justify-between gap-4 shadow-sm hover:shadow-md transition-shadow">
              <div className="flex flex-col gap-1.5 flex-1">
                <span className="text-xs font-semibold uppercase tracking-wider text-blue-600 bg-blue-50 px-2 py-0.5 rounded-full w-max">
                  {getLabelByValue(TIPOS_EVENTO, evento.tipo)}
                </span>
                <p className="font-semibold text-gray-800 text-sm mt-1">
                  {new Date(evento.dataInicio).toLocaleDateString('pt-BR')}
                  {evento.dataInicio !== evento.dataFim && ` até ${new Date(evento.dataFim).toLocaleDateString('pt-BR')}`}
                </p>
                <p className="text-xs text-gray-500">
                  🕒 {evento.horarioInicio.substring(0, 5)} → {evento.horarioFim.substring(0, 5)}
                </p>
                <p className="text-xs text-gray-500">
                  📍 {evento.localizacao} · raio {evento.raioKm}km
                </p>
                <div className="flex items-center gap-2 mt-1 flex-wrap">
                  <span className="text-xs font-semibold uppercase tracking-wider text-purple-600 bg-purple-50 px-2 py-0.5 rounded-full">
                    🎵 {getLabelByValue(FORMATOS, evento.formatoShow)}
                  </span>
                  <span className="text-xs font-medium text-emerald-600 bg-emerald-50 px-2 py-0.5 rounded-full">
                    Até R$ {evento.baseCacheHoraAte}/hora
                  </span>
                  {evento.equipamentoProprio && (
                    <span className="text-xs text-orange-600 bg-orange-50 px-2 py-0.5 rounded-full">
                      🎸 Exige equipamento próprio
                    </span>
                  )}
                </div>
              </div>
              <div className="flex flex-col gap-2 flex-shrink-0">
                <button
                  onClick={() => buscarMatch(evento)}
                  className="px-4 py-2 bg-gradient-to-r from-emerald-500 to-green-600 hover:from-emerald-600 hover:to-green-700 text-white text-sm font-medium rounded-lg transition-all shadow-sm hover:shadow-md"
                >
                  🎯 Match
                </button>
                <button
                  onClick={() => cancelarEvento(evento.id)}
                  className="px-4 py-2 bg-red-50 hover:bg-red-500 hover:text-white text-red-500 text-sm font-medium rounded-lg border border-red-200 hover:border-red-500 transition-colors"
                >
                  Cancelar
                </button>
              </div>
            </div>
          ))}
        </div>
      </div>

      {/* Modal Novo Evento */}
      {showModal && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 px-4">
          <div className="bg-white rounded-2xl shadow-2xl p-6 w-full max-w-md flex flex-col gap-4 max-h-[90vh] overflow-y-auto">
            <div>
              <h3 className="text-lg font-bold text-gray-800">Novo Evento</h3>
              <p className="text-sm text-gray-500 mt-0.5">Preencha os dados do evento que você está organizando</p>
            </div>

            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Tipo de Evento</label>
              <select
                value={novoEvento.tipo}
                onChange={(e) => setNovoEvento({ ...novoEvento, tipo: e.target.value })}
                className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100 bg-white"
              >
                <option value="">Selecione...</option>
                {TIPOS_EVENTO.map(t => <option key={t.value} value={t.value}>{t.label}</option>)}
              </select>
              {errors.tipo && <p className="text-xs text-red-500">{errors.tipo}</p>}
            </div>

            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Formato de Show Procurado</label>
              <select
                value={novoEvento.formatoShow}
                onChange={(e) => setNovoEvento({ ...novoEvento, formatoShow: e.target.value })}
                className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100 bg-white"
              >
                <option value="">Selecione...</option>
                {FORMATOS.map(f => <option key={f.value} value={f.value}>{f.label}</option>)}
              </select>
              {errors.formatoShow && <p className="text-xs text-red-500">{errors.formatoShow}</p>}
            </div>

            <LocationPicker
              label="Local do Evento"
              placeholder="Ex: Belo Horizonte, São Paulo..."
              value={localizacao}
              onChange={setLocalizacao}
              error={errors.localizacao}
            />

            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Raio máximo de busca (km)</label>
              <input
                type="number"
                min="1"
                value={novoEvento.raioKm}
                onChange={(e) => setNovoEvento({ ...novoEvento, raioKm: e.target.value })}
                className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              />
              {errors.raioKm && <p className="text-xs text-red-500">{errors.raioKm}</p>}
            </div>

            <div className="flex gap-3">
              <div className="flex flex-col gap-1 flex-1">
                <label className="text-sm font-medium text-gray-700">Data Início</label>
                <input
                  type="date"
                  min={hoje}
                  value={novoEvento.dataInicio}
                  onChange={(e) => setNovoEvento({ ...novoEvento, dataInicio: e.target.value })}
                  className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                />
                {errors.dataInicio && <p className="text-xs text-red-500">{errors.dataInicio}</p>}
              </div>
              <div className="flex flex-col gap-1 flex-1">
                <label className="text-sm font-medium text-gray-700">Data Fim</label>
                <input
                  type="date"
                  min={novoEvento.dataInicio || hoje}
                  value={novoEvento.dataFim}
                  onChange={(e) => setNovoEvento({ ...novoEvento, dataFim: e.target.value })}
                  className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                />
                {errors.dataFim && <p className="text-xs text-red-500">{errors.dataFim}</p>}
              </div>
            </div>

            <div className="flex gap-3">
              <div className="flex flex-col gap-1 flex-1">
                <label className="text-sm font-medium text-gray-700">Horário Inicial</label>
                <input
                  type="time"
                  value={novoEvento.horarioInicio}
                  onChange={(e) => setNovoEvento({ ...novoEvento, horarioInicio: e.target.value })}
                  className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                />
                {errors.horarioInicio && <p className="text-xs text-red-500">{errors.horarioInicio}</p>}
              </div>
              <div className="flex flex-col gap-1 flex-1">
                <label className="text-sm font-medium text-gray-700">Horário Final</label>
                <input
                  type="time"
                  value={novoEvento.horarioFim}
                  onChange={(e) => setNovoEvento({ ...novoEvento, horarioFim: e.target.value })}
                  className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                />
                {errors.horarioFim && <p className="text-xs text-red-500">{errors.horarioFim}</p>}
              </div>
            </div>

            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Cache máximo por hora (R$)</label>
              <input
                type="number"
                min="0"
                value={novoEvento.baseCacheHoraAte}
                onChange={(e) => setNovoEvento({ ...novoEvento, baseCacheHoraAte: e.target.value })}
                className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
                placeholder="Ex: 300"
              />
              {errors.baseCacheHoraAte && <p className="text-xs text-red-500">{errors.baseCacheHoraAte}</p>}
            </div>

            <div className="flex items-center gap-3">
              <input
                type="checkbox"
                id="equipamento"
                checked={novoEvento.equipamentoProprio}
                onChange={(e) => setNovoEvento({ ...novoEvento, equipamentoProprio: e.target.checked })}
                className="w-4 h-4 accent-blue-600"
              />
              <label htmlFor="equipamento" className="text-sm text-gray-700">
                O artista precisa ter equipamento próprio
              </label>
            </div>

            <div className="flex gap-3 mt-2">
              <button
                onClick={fecharModal}
                className="flex-1 py-2.5 border border-gray-300 text-gray-600 text-sm font-medium rounded-xl hover:bg-gray-50 transition-colors"
              >
                Cancelar
              </button>
              <button
                onClick={criarEvento}
                disabled={salvando}
                className="flex-1 py-2.5 bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 disabled:opacity-50 text-white text-sm font-medium rounded-xl transition-all shadow-md"
              >
                {salvando ? 'Salvando...' : 'Criar Evento'}
              </button>
            </div>
          </div>
        </div>
      )}

      {/* Modal Match */}
      {showMatchModal && (
        <div className="fixed inset-0 bg-black/50 backdrop-blur-sm flex items-center justify-center z-50 px-4">
          <div className="bg-white rounded-2xl shadow-2xl p-6 w-full max-w-lg flex flex-col gap-4 max-h-[85vh]">
            <div className="flex items-center justify-between">
              <div>
                <h3 className="text-lg font-bold text-gray-800">Artistas compatíveis</h3>
                {eventoSelecionado && (
                  <p className="text-xs text-gray-500 mt-0.5">
                    {getLabelByValue(TIPOS_EVENTO, eventoSelecionado.tipo)} · {new Date(eventoSelecionado.dataInicio).toLocaleDateString('pt-BR')} · raio {eventoSelecionado.raioKm}km
                  </p>
                )}
              </div>
              <button
                onClick={() => { setShowMatchModal(false); setMatchResultados([]) }}
                className="text-gray-400 hover:text-gray-600 text-xl font-light">✕
              </button>
            </div>

            <div className="flex flex-col gap-3 overflow-y-auto pr-1">
              {loadingMatch && (
                <p className="text-center text-gray-400 text-sm py-8">🔍 Buscando artistas...</p>
              )}

              {!loadingMatch && matchResultados.length === 0 && (
                <div className="text-center py-8">
                  <p className="text-gray-400 text-sm">Nenhum artista compatível encontrado.</p>
                  <p className="text-gray-300 text-xs mt-1">Tente aumentar o raio ou ajustar os filtros.</p>
                </div>
              )}

              {!loadingMatch && matchResultados.map((artista) => (
                <div key={artista.agendaId} className="border border-gray-200 rounded-2xl p-4 flex items-center justify-between gap-4 shadow-sm hover:shadow-md transition-shadow">
                  <div className="flex flex-col gap-1.5">
                    <p className="font-semibold text-gray-800 text-sm">{artista.nome}</p>
                    {artista.razaoSocial && (
                      <p className="text-xs text-gray-400">{artista.razaoSocial}</p>
                    )}
                    <p className="text-xs text-gray-500">📍 {artista.cidade} · {artista.distanciaKm.toFixed(1)}km</p>
                    {artista.celular1 && (
                      <p className="text-xs text-gray-500">
                        📱 {artista.celular1}{artista.celular2 ? ` / ${artista.celular2}` : ''}
                      </p>
                    )}
                    <div className="flex flex-wrap gap-1 mt-1">
                      {artista.formatosShow.map((f) => (
                        <span key={f} className="text-xs font-semibold uppercase tracking-wider text-purple-600 bg-purple-50 px-2 py-0.5 rounded-full">{f}</span>
                      ))}
                      <span className="text-xs font-medium text-emerald-600 bg-emerald-50 px-2 py-0.5 rounded-full">
                        R$ {artista.baseCacheHora}/hora
                      </span>
                      {artista.equipamentoProprio && (
                        <span className="text-xs text-orange-600 bg-orange-50 px-2 py-0.5 rounded-full">
                          🎸 Equipamento próprio
                        </span>
                      )}
                    </div>
                  </div>
                  <button
                    onClick={() => notificar(artista.artistaId, artista.agendaId, eventoSelecionado!.id)}
                    disabled={notificando === artista.agendaId || jaNotificado(artista.agendaId)}
                    className={`flex-shrink-0 px-3 py-2 text-xs font-medium rounded-lg transition-all ${
                      jaNotificado(artista.agendaId)
                        ? 'bg-gray-100 text-gray-400 border border-gray-200 cursor-not-allowed'
                        : notificando === artista.agendaId
                          ? 'bg-blue-300 text-white cursor-wait'
                          : 'bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 text-white shadow-sm hover:shadow-md'
                    }`}
                  >
                    {jaNotificado(artista.agendaId)
                      ? 'Notificado'
                      : notificando === artista.agendaId
                        ? 'Enviando...'
                        : 'Notificar'}
                  </button>
                </div>
              ))}
            </div>
          </div>
        </div>
      )}
    </div>
  )
}
