import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom'
import { Home } from './pages/Home'
import { CadastrarArtista } from './pages/artista/CadastrarArtista'
import { CadastrarContratante } from './pages/contratante/CadastrarContratante'
import { EscolherTipoUsuario } from './pages/auth/EscolherTipoUsuario'
import { GoogleCallback } from './pages/auth/GoogleCallback'
import { DashboardArtista } from './pages/artista/DashboardArtista'
import { DashboardContratante } from './pages/contratante/DashboardContratante'
import { ProtectedRoute } from './components/ProtectedRoute'
import { EditarPerfilArtista } from './pages/artista/EditarPerfilArtista'
import { EditarPerfilContratante } from './pages/contratante/EditarPerfilContratante'


function App() {
  return (
    <BrowserRouter>
      <Routes>
        {/* Públicas */}
        <Route path="/" element={<Home />} />
        <Route path="/auth/callback" element={<GoogleCallback />} />
        <Route path="/escolher-tipo" element={<EscolherTipoUsuario />} />
        <Route path="/cadastro/artista" element={<CadastrarArtista />} />
        <Route path="/cadastro/contratante" element={<CadastrarContratante />} />

        {/* Protegidas */}
        <Route path="/dashboard/artista" element={
          <ProtectedRoute tipoUsuario="Artista">
            <DashboardArtista />
          </ProtectedRoute>
        } />
        <Route path="/dashboard/contratante" element={
          <ProtectedRoute tipoUsuario="Contratante">
            <DashboardContratante />
          </ProtectedRoute>
        } />
        <Route path="/perfil/artista" element={
          <ProtectedRoute tipoUsuario="Artista">
            <EditarPerfilArtista />
          </ProtectedRoute>
        } />
        <Route path="/perfil/contratante" element={
          <ProtectedRoute tipoUsuario="Contratante">
            <EditarPerfilContratante />
          </ProtectedRoute>
        } />

        <Route path="*" element={<Navigate to="/" replace />} />
      </Routes>
    </BrowserRouter>
  )
}

export default App