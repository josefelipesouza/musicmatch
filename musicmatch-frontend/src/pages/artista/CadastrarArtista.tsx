import { useEffect, useState } from 'react'
import { useLocation, useNavigate } from 'react-router-dom'
import { api } from '../../services/api'

export function CadastrarArtista() {
  const navigate = useNavigate()
  const routeState = useLocation().state as { email?: string; nome?: string } | null

  const [form, setForm] = useState({
    email: routeState?.email ?? '',
    nome: routeState?.nome ?? '',
    cpfCnpj: '',
    razaoSocial: '',
    celular1: '',
    celular2: '',
  })
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [loading, setLoading] = useState(false)
  const [sucesso, setSucesso] = useState(false)

  const validate = () => {
    const e: Record<string, string> = {}
    if (!form.email) e.email = 'Email é obrigatório'
    if (!form.nome) e.nome = 'Nome é obrigatório'
    if (!form.cpfCnpj) e.cpfCnpj = 'CPF/CNPJ é obrigatório'
    if (!form.celular1) e.celular1 = 'Celular 1 é obrigatório'
    setErrors(e)
    return Object.keys(e).length === 0
  }

  const handleSubmit = async () => {
    if (!validate()) return
    setLoading(true)
    try {
      await api.post('/api/artistas', form)
      setSucesso(true)
    } catch (err) {
      console.error(err)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    if (sucesso) navigate('/dashboard/artista')
  }, [sucesso])

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-50 flex items-center justify-center px-4 py-10">
      <div className="max-w-lg w-full bg-white rounded-2xl shadow-2xl p-8 flex flex-col gap-8">

        {/* Header */}
        <div className="text-center">
          <div className="w-16 h-16 mx-auto rounded-xl bg-gradient-to-br from-blue-500 to-indigo-600 flex items-center justify-center text-3xl shadow-md mb-4">
            🎸
          </div>
          <h1 className="text-2xl font-bold text-gray-800">Cadastro de Artista</h1>
          <p className="text-gray-500 mt-1 text-sm">
            {routeState?.nome
              ? `Olá, ${routeState.nome.split(' ')[0]}! Complete seu perfil para começar.`
              : 'Você poderá adicionar localização e tipo de show ao criar seus agendamentos.'}
          </p>
        </div>

        {/* Campos */}
        <div className="flex flex-col gap-4">
          {/* Email */}
          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">Email</label>
            <input
              type="email"
              value={form.email}
              readOnly={!!routeState?.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
              placeholder="seu@email.com"
              className={`px-4 py-2.5 rounded-xl border text-sm outline-none transition-all ${
                routeState?.email
                  ? 'border-gray-200 bg-gray-50 text-gray-400 cursor-not-allowed'
                  : 'border-gray-200 focus:border-blue-400 focus:ring-2 focus:ring-blue-100'
              }`}
            />
            {errors.email && <p className="text-xs text-red-500">{errors.email}</p>}
          </div>

          {/* Nome artístico */}
          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">Nome artístico</label>
            <input
              type="text"
              value={form.nome}
              onChange={(e) => setForm({ ...form, nome: e.target.value })}
              placeholder="Nome ou nome da banda"
              className="px-4 py-2.5 rounded-xl border border-gray-200 text-sm outline-none transition-all focus:border-blue-400 focus:ring-2 focus:ring-blue-100"
            />
            {errors.nome && <p className="text-xs text-red-500">{errors.nome}</p>}
          </div>

          {/* CPF/CNPJ */}
          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">CPF / CNPJ</label>
            <input
              type="text"
              value={form.cpfCnpj}
              onChange={(e) => setForm({ ...form, cpfCnpj: e.target.value })}
              placeholder="000.000.000-00"
              className="px-4 py-2.5 rounded-xl border border-gray-200 text-sm outline-none transition-all focus:border-blue-400 focus:ring-2 focus:ring-blue-100"
            />
            {errors.cpfCnpj && <p className="text-xs text-red-500">{errors.cpfCnpj}</p>}
          </div>

          {/* Razão Social */}
          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">
              Razão Social <span className="text-gray-400 font-normal">(opcional)</span>
            </label>
            <input
              type="text"
              value={form.razaoSocial}
              onChange={(e) => setForm({ ...form, razaoSocial: e.target.value })}
              className="px-4 py-2.5 rounded-xl border border-gray-200 text-sm outline-none transition-all focus:border-blue-400 focus:ring-2 focus:ring-blue-100"
            />
          </div>

          {/* Celulares */}
          <div className="grid grid-cols-2 gap-3">
            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">Celular 1</label>
              <input
                type="tel"
                value={form.celular1}
                onChange={(e) => setForm({ ...form, celular1: e.target.value })}
                placeholder="(00) 90000-0000"
                className="px-4 py-2.5 rounded-xl border border-gray-200 text-sm outline-none transition-all focus:border-blue-400 focus:ring-2 focus:ring-blue-100"
              />
              {errors.celular1 && <p className="text-xs text-red-500">{errors.celular1}</p>}
            </div>

            <div className="flex flex-col gap-1">
              <label className="text-sm font-medium text-gray-700">
                Celular 2 <span className="text-gray-400 font-normal">(opcional)</span>
              </label>
              <input
                type="tel"
                value={form.celular2}
                onChange={(e) => setForm({ ...form, celular2: e.target.value })}
                placeholder="(00) 90000-0000"
                className="px-4 py-2.5 rounded-xl border border-gray-200 text-sm outline-none transition-all focus:border-blue-400 focus:ring-2 focus:ring-blue-100"
              />
            </div>
          </div>
        </div>

        {/* Botão */}
        <button
          onClick={handleSubmit}
          disabled={loading}
          className="w-full py-3 rounded-xl bg-gradient-to-r from-blue-500 to-indigo-600 hover:from-blue-600 hover:to-indigo-700 disabled:opacity-50 text-white font-semibold shadow-md transition-all"
        >
          {loading ? 'Cadastrando...' : 'Criar perfil'}
        </button>

        {routeState?.email && (
          <p className="text-center text-xs text-gray-400">
            Conectado com <span className="font-medium text-gray-500">{routeState.email}</span>
          </p>
        )}
      </div>
    </div>
  )
}