import { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../../hooks/useAuth'
import { api } from '../../services/api'

export function EditarPerfilContratante() {
  const { userId, token } = useAuth()
  const navigate = useNavigate()
  const headers = { Authorization: `Bearer ${token}` }

  const [form, setForm] = useState({
    cpfCnpj: '',
    razaoSocial: '',
    celular1: '',
    celular2: '',
  })
  const [loading, setLoading] = useState(true)
  const [salvando, setSalvando] = useState(false)
  const [errors, setErrors] = useState<Record<string, string>>({})
  const [sucesso, setSucesso] = useState(false)

  useEffect(() => {
    if (!userId) return
    api.get(`/api/contratantes/${userId}`, { headers })
      .then(res => {
        const d = res.data
        setForm({
          cpfCnpj: d.cpfCnpj ?? '',
          razaoSocial: d.razaoSocial ?? '',
          celular1: d.celular1 ?? '',
          celular2: d.celular2 ?? '',
        })
      })
      .finally(() => setLoading(false))
  }, [userId])

  const validate = () => {
    const e: Record<string, string> = {}
    if (!form.cpfCnpj) e.cpfCnpj = 'CPF/CNPJ é obrigatório'
    if (!form.celular1) e.celular1 = 'Celular 1 é obrigatório'
    setErrors(e)
    return Object.keys(e).length === 0
  }

  const handleSubmit = async () => {
    if (!validate() || !userId) return
    setSalvando(true)
    try {
      await api.put(`/api/contratantes/${userId}`, {
        id: userId,
        cpfCnpj: form.cpfCnpj,
        razaoSocial: form.razaoSocial || null,
        celular1: form.celular1,
        celular2: form.celular2 || null,
      }, { headers })
      setSucesso(true)
      setTimeout(() => navigate('/dashboard/contratante'), 1500)
    } catch (err) {
      console.error(err)
    } finally {
      setSalvando(false)
    }
  }

  if (loading) return (
    <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-slate-50 to-blue-50">
      <p className="text-gray-400 text-sm">Carregando...</p>
    </div>
  )

  return (
    <div className="min-h-screen bg-gradient-to-br from-slate-50 via-blue-50 to-indigo-50 py-10 px-4">
      <div className="max-w-lg mx-auto bg-white rounded-2xl shadow-lg p-8 flex flex-col gap-6">
        <div className="flex items-center gap-3">
          <button
            onClick={() => navigate('/dashboard/contratante')}
            className="text-gray-400 hover:text-gray-600 transition-colors"
          >
            ← Voltar
          </button>
          <div className="flex items-center gap-3">
            <div className="w-11 h-11 rounded-xl bg-gradient-to-br from-blue-600 to-indigo-600 flex items-center justify-center text-white text-lg shadow-md shrink-0">
              ♪
            </div>
            <div>
              <h1 className="text-2xl font-bold text-gray-800">Editar Perfil</h1>
              <p className="text-sm text-gray-500 mt-0.5">Atualize suas informações de contato</p>
            </div>
          </div>
        </div>

        {sucesso && (
          <div className="bg-emerald-50 border border-emerald-200 text-emerald-700 px-4 py-3 rounded-xl text-sm font-medium">
            ✅ Perfil atualizado com sucesso! Redirecionando...
          </div>
        )}

        <div className="flex flex-col gap-4">
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

        <div className="flex gap-3">
          <button
            onClick={() => navigate('/dashboard/contratante')}
            className="flex-1 py-2.5 border border-gray-300 text-gray-600 text-sm font-medium rounded-xl hover:bg-gray-50 transition-colors"
          >
            Cancelar
          </button>
          <button
            onClick={handleSubmit}
            disabled={salvando || sucesso}
            className="flex-1 py-2.5 bg-gradient-to-r from-blue-600 to-indigo-600 hover:from-blue-700 hover:to-indigo-700 disabled:opacity-50 text-white text-sm font-medium rounded-xl transition-all shadow-md"
          >
            {salvando ? 'Salvando...' : 'Salvar'}
          </button>
        </div>
      </div>
    </div>
  )
}
