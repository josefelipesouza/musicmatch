interface RaioSliderProps {
  value: number
  onChange: (value: number) => void
}

const OPCOES = [25, 50, 100, 150, 200, 300]

export function RaioSlider({ value, onChange }: RaioSliderProps) {
  return (
    <div className="flex flex-col gap-3">
      <div className="flex justify-between items-center">
        <label className="text-sm font-medium text-gray-700">
          Raio de busca
        </label>
        <span className="text-sm font-semibold text-blue-600 bg-blue-50 px-3 py-1 rounded-full">
          {value} km
        </span>
      </div>

      <input
        type="range"
        min={0}
        max={OPCOES.length - 1}
        step={1}
        value={OPCOES.indexOf(value) === -1 ? 2 : OPCOES.indexOf(value)}
        onChange={(e) => onChange(OPCOES[parseInt(e.target.value)])}
        className="w-full h-2 bg-gray-200 rounded-lg appearance-none cursor-pointer accent-blue-600"
      />

      {/* Labels dos valores */}
      <div className="flex justify-between text-xs text-gray-400">
        {OPCOES.map((km) => (
          <span
            key={km}
            className={`cursor-pointer transition-colors ${value === km ? 'text-blue-600 font-semibold' : ''}`}
            onClick={() => onChange(km)}
          >
            {km}km
          </span>
        ))}
      </div>

      {/* Descrição do raio */}
      <p className="text-xs text-gray-500">
        {value <= 50
          ? '📍 Região local — artistas bem próximos'
          : value <= 100
          ? '🗺️ Região ampla — cidades vizinhas incluídas'
          : '🌎 Região extensa — pode incluir outros estados'}
      </p>
    </div>
  )
}