import { useEffect, useState } from "react";
import type {
  TurnoDtoResponse,
  TurnosTableProps,
} from "../../interface/TurnosType";

import { deleteTurno, getHorarioAtencionUsuario } from "../../service/api";
import toast from "react-hot-toast";

export const TurnosTable: React.FC<TurnosTableProps> = ({ idUsuario }) => {
  const [loading, setLoading] = useState(false);
  const [turnos, setTurnos] = useState<TurnoDtoResponse[]>([]);

  useEffect(() => {
    const fetchTurnos = async () => {
      setLoading(true);
      try {
        const data = await getHorarioAtencionUsuario(idUsuario);
        console.log("Respuesta backend:", data);
        setTurnos(data);
      } catch (error) {
        console.error("Error al obtener turnos del usuario:", error);
      } finally {
        setLoading(false);
      }
    };
    if (idUsuario) {
      fetchTurnos();
    }
  }, [idUsuario]);

  const handleCancelar = async (turnoId: string) => {
    const confirmacion = window.confirm(
      "¿Estás seguro de que deseas cancelar este turno?",
    );
    if (!confirmacion) return;

    try {
      const response = await deleteTurno(turnoId, idUsuario);
      toast.success(response.message);

      setTurnos((prevTurnos) => prevTurnos.filter((t) => t.id !== turnoId));
    } catch (error: unknown) {
      const errorObject = error as Error;
      toast.error(errorObject.message);
    }
  };

  if (loading) return <p>Cargando turnos...</p>;
  return (
    <div>
      <h2>Turnos del usuario</h2>
      {turnos.length === 0 ? (
        <p>No tenés turnos reservados actualmente.</p>
      ) : (
        <table>
          <thead>
            <tr>
              <th>Local</th>
              <th>Servicio</th>
              <th>Fecha</th>
              <th>Acción</th>
            </tr>
          </thead>
          <tbody>
            {turnos.map((turno) => (
              <tr key={turno.id}>
                <td>{turno.localId}</td>
                <td>{turno.servicioId}</td>
                <td>{new Date(turno.date).toLocaleDateString()}</td>
                <td>
                  <button onClick={() => handleCancelar(turno.id)}>
                    Cancelar turno
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      )}
    </div>
  );
};
