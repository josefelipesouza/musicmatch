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
    <div className="min-h-screen bg-gray-50 py-10 px-4">
      <div className="max-w-lg mx-auto bg-white rounded-2xl shadow p-8 flex flex-col gap-6">
        <div>
          <h1 className="text-2xl font-semibold text-gray-800">Cadastro de Artista</h1>
          <p className="text-sm text-gray-500 mt-1">
            Você poderá adicionar localização e tipo de show ao criar seus agendamentos.
          </p>
        </div>

        <div className="flex flex-col gap-4">
          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">Email</label>
            <input
              type="email"
              value={form.email}
              readOnly={!!routeState?.email}
              onChange={(e) => setForm({ ...form, email: e.target.value })}
              className={`px-4 py-2.5 rounded-lg border text-sm outline-none ${
                routeState?.email
                  ? 'border-gray-200 bg-gray-50 text-gray-400 cursor-not-allowed'
                  : 'border-gray-300 focus:border-blue-500 focus:ring-2 focus:ring-blue-100'
              }`}
              placeholder="seu@email.com"
            />
            {errors.email && <p className="text-xs text-red-500">{errors.email}</p>}
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">Nome artístico</label>
            <input
              type="text"
              value={form.nome}
              onChange={(e) => setForm({ ...form, nome: e.target.value })}
              className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="Nome ou nome da banda"
            />
            {errors.nome && <p className="text-xs text-red-500">{errors.nome}</p>}
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">CPF / CNPJ</label>
            <input
              type="text"
              value={form.cpfCnpj}
              onChange={(e) => setForm({ ...form, cpfCnpj: e.target.value })}
              className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="000.000.000-00"
            />
            {errors.cpfCnpj && <p className="text-xs text-red-500">{errors.cpfCnpj}</p>}
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">
              Razão Social <span className="text-gray-400">(opcional)</span>
            </label>
            <input
              type="text"
              value={form.razaoSocial}
              onChange={(e) => setForm({ ...form, razaoSocial: e.target.value })}
              className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
            />
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">Celular 1</label>
            <input
              type="tel"
              value={form.celular1}
              onChange={(e) => setForm({ ...form, celular1: e.target.value })}
              className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="(00) 90000-0000"
            />
            {errors.celular1 && <p className="text-xs text-red-500">{errors.celular1}</p>}
          </div>

          <div className="flex flex-col gap-1">
            <label className="text-sm font-medium text-gray-700">
              Celular 2 <span className="text-gray-400">(opcional)</span>
            </label>
            <input
              type="tel"
              value={form.celular2}
              onChange={(e) => setForm({ ...form, celular2: e.target.value })}
              className="px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
              placeholder="(00) 90000-0000"
            />
          </div>
        </div>

        <button
          onClick={handleSubmit}
          disabled={loading}
          className="w-full py-3 bg-blue-600 hover:bg-blue-700 disabled:bg-blue-300 text-white font-medium rounded-xl transition-colors"
        >
          {loading ? 'Cadastrando...' : 'Criar perfil'}
        </button>
      </div>
    </div>
  )
}