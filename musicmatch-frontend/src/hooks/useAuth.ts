export function useAuth() {
  const token = localStorage.getItem('token')
  const tipoUsuario = localStorage.getItem('tipoUsuario')

  if (!token) return { token: null, userId: null, tipoUsuario: null }

  try {
    const payload = JSON.parse(atob(token.split('.')[1]))
    const userId = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier']
    return { token, userId, tipoUsuario }
  } catch {
    return { token: null, userId: null, tipoUsuario: null }
  }
}