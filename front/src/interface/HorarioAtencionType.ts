
export const DayOfWeek = {
  0: 'Domingo',
  1: 'Lunes',
  2: 'Martes',
  3: 'Miércoles',
  4: 'Jueves',
  5: 'Viernes',
  6: 'Sábado'
} as const;

export interface HorarioAtencionType {
    localId: string;
    diaSemana: keyof typeof DayOfWeek;
    horaApertura: string; // Formato "HH:mm"
    horaCierre: string;  // Formato "HH:mm"
    estaCerrado: boolean;
}