import { useState, useEffect, useRef } from 'react';
import type { LocationData } from '../../types';

interface CidadeJSON {
  codigo_ibge: number;
  nome: string;
  latitude: number;
  longitude: number;
  capital: number;
  codigo_uf: number;
  siafi_id: number;
  ddd: number;
  fuso_horario: string;
}

interface EstadoJSON {
  codigo_uf: number;
  uf: string;
  nome: string;
  latitude: number;
  longitude: number;
  regiao: string;
}

interface LocationPickerProps {
  label: string;
  placeholder: string;
  value: LocationData | null;
  onChange: (data: LocationData | null) => void;
  error?: string;
}

export function LocationPicker({ label, placeholder, value, onChange, error }: LocationPickerProps) {
  const [busca, setBusca] = useState(value ? value.cidade : '');
  const [sugestoes, setSugestoes] = useState<LocationData[]>([]);
  const [todasCidades, setTodasCidades] = useState<CidadeJSON[]>([]);
  const [estados, setEstados] = useState<Record<number, string>>({});
  const [carregando, setCarregando] = useState(false);
  const wrapperRef = useRef<HTMLDivElement>(null);

  // Carrega os JSONs estáticos da pasta public ao montar o componente
  useEffect(() => {
    async function carregarDados() {
      setCarregando(true);
      try {
        const [resEstados, resCidades] = await Promise.all([
          fetch('/estados-brasil.json'),
          fetch('/cidades-brasil.json')
        ]);

        if (resEstados.ok && resCidades.ok) {
          const dadosEstados: EstadoJSON[] = await resEstados.json();
          const dadosCidades: CidadeJSON[] = await resCidades.json();

          // Mapeia codigo_uf -> UF (ex: 31 -> "MG") para busca rápida
          const mapaEstados: Record<number, string> = {};
          dadosEstados.forEach(est => {
            mapaEstados[est.codigo_uf] = est.uf;
          });

          setEstados(mapaEstados);
          setTodasCidades(dadosCidades);
        }
      } catch (err) {
        console.error("Erro ao carregar dados de localização estáticos:", err);
      } finally {
        setCarregando(false);
      }
    }

    carregarDados();
  }, []);

  // Fecha a lista de sugestões se clicar fora do componente
  useEffect(() => {
    function escutarCliqueFora(event: MouseEvent) {
      if (wrapperRef.current && !wrapperRef.current.contains(event.target as Node)) {
        setSugestoes([]);
      }
    }
    document.addEventListener('mousedown', escutarCliqueFora);
    return () => document.removeEventListener('mousedown', escutarCliqueFora);
  }, []);

  // Sincroniza o input quando o valor externo for resetado (ex: ao fechar/limpar modal)
  useEffect(() => {
    setBusca(value ? value.cidade : '');
  }, [value]);

  // Filtra as cidades localmente conforme a digitação
  const handleInputChange = (texto: string) => {
    setBusca(texto);

    if (texto.trim().length < 2 || todasCidades.length === 0) {
      setSugestoes([]);
      return;
    }

    const termoNorm = texto.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");

    const filtradas = todasCidades
      .filter(cidade => {
        const nomeNorm = cidade.nome.toLowerCase().normalize("NFD").replace(/[\u0300-\u036f]/g, "");
        return nomeNorm.includes(termoNorm);
      })
      .slice(0, 6) // Limita a 6 sugestões para performance visual
      .map(cidade => {
        const siglaUf = estados[cidade.codigo_uf] || '';
        return {
          cidade: `${cidade.nome} - ${siglaUf}`,
          latitude: cidade.latitude,
          longitude: cidade.longitude
        };
      });

    setSugestoes(filtradas);
  };

  const selecionarSugestao = (item: LocationData) => {
    setBusca(item.cidade);
    setSugestoes([]);
    onChange(item); // Repassa cidade, lat e lng para o Dashboard pai
  };

  return (
    <div ref={wrapperRef} className="flex flex-col gap-1 relative w-full">
      <label className="text-sm font-medium text-gray-700">{label}</label>
      
      <div className="relative">
        <input
          type="text"
          value={busca}
          onChange={(e) => handleInputChange(e.target.value)}
          placeholder={carregando ? "Carregando lista de cidades..." : placeholder}
          disabled={carregando}
          className="w-full px-4 py-2.5 rounded-lg border border-gray-300 text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-100"
        />
        {carregando && (
          <span className="absolute right-3 top-3 text-xs text-gray-400 animate-pulse">
            ...
          </span>
        )}
      </div>

      {error && <p className="text-xs text-red-500">{error}</p>}

      {/* Dropdown de Sugestões */}
      {sugestoes.length > 0 && (
        <ul className="absolute z-50 left-0 right-0 top-[calc(100%+4px)] bg-white border border-gray-200 rounded-xl shadow-lg max-h-60 overflow-y-auto py-1">
          {sugestoes.map((item, index) => (
            <li key={index}>
              <button
                type="button"
                onClick={() => selecionarSugestao(item)}
                className="w-full text-left px-4 py-2 text-sm text-gray-700 hover:bg-gray-50 focus:bg-gray-50 outline-none transition-colors"
              >
                📍 {item.cidade}
              </button>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}