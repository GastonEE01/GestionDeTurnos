import { useEffect, useState } from "react";
import type { ServicioDtoResponse } from "../../interface/ServicioType.ts";
import { addTurno, getHorarioAtencionDisponible } from "../../service/api.ts";
import toast from "react-hot-toast";
import { X, Calendar, Clock } from "lucide-react";

interface ModalTurnoProps {
  cerrar: () => void;
  localId: string;
  servicios: ServicioDtoResponse[];
  usuarioId: string;
}
export const ModalTurno = ({
  cerrar,
  usuarioId,
  localId,
  servicios,
}: ModalTurnoProps) => {
  const [loading, setLoading] = useState(false);
  const [fechaSeleccionada, setFechaSeleccionada] = useState<string>("");
  const [turnosDisponibles, setTurnosDisponibles] = useState<string[]>([]);
  const [servicioSeleccionadoId, setServicioSeleccionadoId] = useState<string>("");
  const [horaSeleccionada, setHoraSeleccionada] = useState<string | null>(null);
  useEffect(() => {
    if (!localId || !servicioSeleccionadoId || !fechaSeleccionada) return;

    const fetchTurnos = async () => {
      setLoading(true);
      try {
        const data = await getHorarioAtencionDisponible(
          localId,
          servicioSeleccionadoId,
          fechaSeleccionada,
        );
        setTurnosDisponibles(data);
      } catch (error) {
        console.error("Error al obtener turnos:", error);
      } finally {
        setLoading(false);
      }
    };
    fetchTurnos();
  }, [localId, servicioSeleccionadoId, fechaSeleccionada]);

  const servicioActual = servicios.find((s) => s.id === servicioSeleccionadoId);
//  async (horaSeleccionada: string) => {
  const handleConfirmarReserva = async () => {
    if (!horaSeleccionada || !fechaSeleccionada || !servicioSeleccionadoId) {
      toast.error("Selecciona servicio, fecha y horario");
      return;
    }
    const confirmacion = window.confirm(
      `¿Confirmar turno para el día ${fechaSeleccionada} a las ${horaSeleccionada} hs?`,
    );
    if (!confirmacion) return;

    try {
      // Unimos la fecha y la hora elegida (ejemplo: "2026-08-28T09:30:00")
      const horaFormateada =
        horaSeleccionada.length === 5
          ? `${horaSeleccionada}:00`
          : horaSeleccionada;
      const fechaHoraInicio = `${fechaSeleccionada}T${horaFormateada}Z`;
      const response = await addTurno({
        usuarioId: usuarioId, // En lugar de clienteId
        localId: localId,
        servicioId: servicioSeleccionadoId,
        date: fechaHoraInicio,
      });
      toast.success(response.message);
      cerrar();
    } catch (error: unknown) {
      const errorObject = error as Error;
      toast.error(errorObject.message);
    }
  };

  return (
   <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/40 backdrop-blur-sm p-4">
      <div className="bg-white rounded-3xl shadow-xl w-full max-w-md p-6 relative border border-gray-100 animate-in fade-in zoom-in duration-200">

      {/* Header */}
        <div className="flex justify-between items-start mb-5">
          <div>
            <span className="text-xs font-bold uppercase tracking-wider text-sky-900">
              RESERVAR TURNO
            </span>
            <h2 className="text-2xl font-black text-gray-900 leading-tight">
              Reservar Turno
            </h2>
            <p className="text-sm text-gray-500 mt-0.5">
              Elegí servicio, fecha y un bloque libre.
            </p>
          </div>
          <button
            onClick={cerrar}
            className="text-gray-400 hover:text-gray-600 transition-colors p-1"
          >
            <X size={20} />
          </button>
        </div>

        <div className="space-y-4">
          {/* Select Servicio */}
          <div>
            <label className="block text-xs font-semibold text-gray-700 mb-1">
              Servicio
            </label>
            <select
              value={servicioSeleccionadoId}
              onChange={(e) => setServicioSeleccionadoId(e.target.value)}
              className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl px-3 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500 transition-all cursor-pointer"
            >
              <option value="">-- Seleccioná un servicio --</option>
              {servicios.map((s) => (
                <option key={s.id} value={s.id}>
                  {s.name}
                </option>
              ))}
            </select>
          </div>

          {/* Input Fecha */}
          <div>
            <label className="block text-xs font-semibold text-gray-700 mb-1">
              Fecha
            </label>
            <div className="relative">
              <input
                type="date"
                value={fechaSeleccionada}
                onChange={(e) => setFechaSeleccionada(e.target.value)}
                className="w-full bg-sky-50/50 border border-sky-100 text-gray-800 rounded-xl px-3 py-2.5 text-sm focus:outline-none focus:ring-2 focus:ring-sky-500 transition-all cursor-pointer"
              />
              <Calendar className="absolute right-3 top-3 text-gray-400 pointer-events-none" size={16} />
            </div>
          </div>

          {/* Horarios Disponibles */}
          <div>
            <label className="block text-xs font-semibold text-gray-700 mb-2">
              Horarios disponibles {servicioActual?.durationInMinutes ? `· ${servicioActual.durationInMinutes} min` : ""}
            </label>

            {loading && (
              <p className="text-xs text-sky-800 bg-sky-50 p-3 rounded-xl flex items-center gap-2">
                <Clock size={16} className="animate-spin" /> Cargando horarios libres...
              </p>
            )}

            {!loading && turnosDisponibles.length > 0 && (
              <div className="grid grid-cols-4 gap-2 max-h-48 overflow-y-auto pr-1">
                {turnosDisponibles.map((hora) => {
                  const isSelected = horaSeleccionada === hora;
                  return (
                    <button
                      key={hora}
                      type="button"
                      onClick={() => setHoraSeleccionada(hora)}
                      className={`py-2 px-1 text-xs font-medium rounded-xl border transition-all ${
                        isSelected
                          ? "bg-sky-900 text-white border-sky-900 shadow-sm"
                          : "bg-white text-gray-700 border-sky-100 hover:border-sky-300 hover:bg-sky-50/30"
                      }`}
                    >
                      {hora} hs
                    </button>
                  );
                })}
              </div>
            )}

            {!loading && fechaSeleccionada && servicioSeleccionadoId && turnosDisponibles.length === 0 && (
              <p className="text-xs text-amber-800 bg-amber-50 p-3 rounded-xl">
                No hay turnos disponibles para esta fecha.
              </p>
            )}

            {!fechaSeleccionada || !servicioSeleccionadoId ? (
              <p className="text-xs text-gray-400 italic">
                Seleccioná servicio y fecha para consultar los bloques disponibles.
              </p>
            ) : null}
          </div>

          {/* Botones de Acción */}
          <div className="flex items-center gap-3 pt-3">
            <button
              type="button"
              onClick={cerrar}
              className="w-1/2 border border-sky-100 text-gray-700 font-semibold py-3 px-4 rounded-xl text-sm hover:bg-gray-50 transition-colors"
            >
              Cancelar
            </button>
            <button
              type="button"
              disabled={loading || !horaSeleccionada}
              onClick={handleConfirmarReserva}
              className={`w-1/2 font-semibold py-3 px-4 rounded-xl text-sm transition-all shadow-sm ${
                !horaSeleccionada || loading
                  ? "bg-amber-200 text-amber-800/60 cursor-not-allowed"
                  : "bg-amber-300 hover:bg-amber-400 text-amber-950"
              }`}
            >
              Confirmar turno
            </button>
          </div>
        </div>

      </div>
    </div>
  );
};


/*

 {/* 1° Elige Servicio }*/
 /*
      <select onChange={(e) => setServicioSeleccionadoId(e.target.value)}>
        <option value="">-- Seleccioná un servicio --</option>
        {servicios.map((s) => (
          <option key={s.id} value={s.id}>
            {s.name}
          </option>
        ))}
      </select>

      {/* 2° Elige Fecha }*/
      /*
      <input
        type="date"
        value={fechaSeleccionada}
        onChange={(e) => setFechaSeleccionada(e.target.value)}
      />

      {/* 3° Lista de Horarios *}/
      /*{loading && <p>Cargando horarios libres...</p>}

      <div className="horarios-grid">
        {!loading &&
          turnosDisponibles.length > 0 &&
          turnosDisponibles.map((hora) => (
            <button key={hora} onClick={() => setHoraSeleccionada(hora)}>
              {hora} hs
            </button>
          ))}

        {!loading &&
          fechaSeleccionada &&
          servicioSeleccionadoId &&
          turnosDisponibles.length === 0 && (
            <p>No hay turnos disponibles para esta fecha.</p>
          )}
      </div>

      <button onClick={cerrar}>Cancelar</button>
    </div>*/