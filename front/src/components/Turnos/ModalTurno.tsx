import { useEffect, useState } from "react";
import type { ServicioDtoResponse } from "../../interface/ServicioType.ts";
import { addTurno, getHorarioAtencionDisponible } from "../../service/api.ts";
import toast from "react-hot-toast";

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
  const [servicioSeleccionadoId, setServicioSeleccionadoId] =
    useState<string>("");

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

  const handleConfirmarReserva = async (horaSeleccionada: string) => {
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
    } catch (error: any) {
      toast.error(error.message);
    }
  };

  return (
    <div className="modal">
      <h2>Reservar Turno</h2>

      {/* 1° Elige Servicio */}
      <select onChange={(e) => setServicioSeleccionadoId(e.target.value)}>
        <option value="">-- Seleccioná un servicio --</option>
        {servicios.map((s) => (
          <option key={s.id} value={s.id}>
            {s.name}
          </option>
        ))}
      </select>

      {/* 2° Elige Fecha */}
      <input
        type="date"
        value={fechaSeleccionada}
        onChange={(e) => setFechaSeleccionada(e.target.value)}
      />

      {/* 3° Lista de Horarios */}
      {loading && <p>Cargando horarios libres...</p>}

      <div className="horarios-grid">
        {!loading &&
          turnosDisponibles.length > 0 &&
          turnosDisponibles.map((hora) => (
            <button key={hora} onClick={() => handleConfirmarReserva(hora)}>
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
    </div>
  );
};
