export const FormatoShow = {
  Banda: 1,
  CantorSolo: 2,
  Coral: 3,
  Baterista: 4,
  Guitarrista: 5,
  Baixista: 6,
  Tecladista: 7,
  Violonista: 8,
  DJ: 9,
  Outro: 10,
} as const

export type FormatoShow = typeof FormatoShow[keyof typeof FormatoShow]

export const TipoEvento = {
  Show: 1,
  Festa: 2,
  Casamento: 3,
  Corporativo: 4,
  Festival: 5,
  Aniversario: 6,
  Formatura: 7,
  Outro: 8,
} as const

export type TipoEvento = typeof TipoEvento[keyof typeof TipoEvento]

export interface LocationData {
  cidade: string
  latitude: number
  longitude: number
}

export interface ArtistaDto {
  id: string
  email: string
  nome: string
  cpfCnpj: string
  razaoSocial?: string
  cidade: string
  latitude: number
  longitude: number
  equipamentoProprio: boolean
  formatosShow: string[]
  criadoEm: string
}

export interface MatchDto {
  artistaId: string
  nome: string
  cidade: string
  distanciaKm: number
  equipamentoProprio: boolean
  formatosShow: string[]
  baseCacheHora: number
}

export interface EventoDto {
  id: string
  contratanteId: string
  localizacao: string
  latitude: number
  longitude: number
  raioKm: number
  formatoShow: string
  tipo: string
  dataInicio: string
  dataFim: string
  horarioInicio: string
  horarioFim: string
  equipamentoProprio: boolean
  baseCacheHoraAte: number
  criadoEm: string
}