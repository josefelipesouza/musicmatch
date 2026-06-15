import { useEffect } from 'react'
import { useNavigate } from 'react-router-dom'

export function GoogleCallback() {
  const navigate = useNavigate()

  useEffect(() => {
    const params = new URLSearchParams(window.location.search)
    const token = params.get('token')
    const tipo = params.get('tipo')
    const primeiroAcesso = params.get('primeiroAcesso')
    const email = params.get('email')
    const nome = params.get('nome')

    if (primeiroAcesso === 'true') {
      navigate('/escolher-tipo', { state: { email, nome } })
    } else if (token && tipo) {
      localStorage.setItem('token', token)
      localStorage.setItem('tipoUsuario', tipo)
      if (tipo === 'Artista') {
        navigate('/dashboard/artista')
      } else {
        navigate('/dashboard/contratante')
      }
    }
    
  }, [])

  return (
    <div className="min-h-screen flex items-center justify-center bg-gray-50">
      <p className="text-gray-500 text-sm">Autenticando...</p>
    </div>
  )
}