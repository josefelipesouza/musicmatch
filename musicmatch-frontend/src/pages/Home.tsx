import bannerHome from '../assets/banner-home.png'

export function Home() {
  const handleLogin = () => {
    window.location.href = 'http://localhost:5216/api/auth/google'
  }
  const handleCadastro = () => {
    window.location.href = 'http://localhost:5216/api/auth/google?origem=cadastro'
  }

  return (
    <div className="min-h-screen relative overflow-hidden flex flex-col items-center justify-end px-4 pb-[7%]">

      {/* Banner de fundo */}
      <div
        className="absolute inset-0 w-full h-full"
        style={{
          zIndex: 0,
          backgroundImage: `url(${bannerHome})`,
          backgroundSize: 'cover',
          backgroundPosition: 'center',
        }}
      />

      {/* Botões */}
      <div className="relative z-10 flex flex-col gap-3 w-full max-w-sm">
        <button
          onClick={handleLogin}
          className="w-full py-5 bg-blue-600 hover:bg-blue-700 text-white font-medium rounded-xl transition-colors text-base shadow-lg"
        >
          Já tenho conta
        </button>
        <button
          onClick={handleCadastro}
          className="w-full py-5 bg-white hover:bg-gray-100 text-gray-800 font-medium rounded-xl transition-colors text-base shadow-lg"
        >
          Quero me cadastrar
        </button>
      </div>
    </div>
  )
}
