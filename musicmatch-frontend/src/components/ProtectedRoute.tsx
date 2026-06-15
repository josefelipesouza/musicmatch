import { Navigate } from 'react-router-dom'
import { useAuth } from '../hooks/useAuth'

interface Props {
  children: React.ReactNode
  tipoUsuario?: 'Artista' | 'Contratante'
}

export function ProtectedRoute({ children, tipoUsuario }: Props) {
  const { token, tipoUsuario: tipo } = useAuth()

  if (!token) {
    return <Navigate to="/" replace />
  }

  if (tipoUsuario && tipo !== tipoUsuario) {
    return <Navigate to="/" replace />
  }

  return <>{children}</>
}