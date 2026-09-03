import { useEffect, useState } from "react";
import { X } from "lucide-react";
import type {
  TurnoDtoResponse,
  TurnosTableProps,
} from "../../interface/TurnosType";

import {
  deleteTurno,
  getTurnosPorLocal,
  getHorarioAtencionUsuario,
} from "../../service/api";
import toast from "react-hot-toast";

export const TurnosTable: React.FC<TurnosTableProps> = ({
  idUsuario,
  idLocal,
}) => {
  const [loading, setLoading] = useState(false);
  const [turnos, setTurnos] = useState<TurnoDtoResponse[]>([]);

  useEffect(() => {
    const fetchTurnos = async () => {
      setLoading(true);
      try {
        let data: TurnoDtoResponse[] = [];
        if (idLocal) {
          data = await getTurnosPorLocal(idLocal);
        } else if (idUsuario) {
          data = await getHorarioAtencionUsuario(idUsuario);
        }

        console.log("Respuesta backend:", data);
        setTurnos(data);
      } catch (error) {
        console.error("Error al obtener turnos:", error);
      } finally {
        setLoading(false);
      }
    };

    if (idUsuario || idLocal) {
      fetchTurnos();
    }
  }, [idUsuario, idLocal]);

  const handleCancel = async (turnoId: string) => {
    if (confirm("¿Estás seguro de que querés cancelar este turno?")) {
      try {
        const targetId = idUsuario || idLocal || "";
        const response = await deleteTurno(turnoId, targetId);
        toast.success(response.message);
        setTurnos((prevTurnos) => prevTurnos.filter((t) => t.id !== turnoId));
      } catch (error) {
        const errorObject = error as Error;
        toast.error(errorObject.message);
      }
    }
  };

  const formatDate = (dateStr: string) => {
    const dateObj = new Date(dateStr);
    if (isNaN(dateObj.getTime())) {
      return { day: "--", month: "---" };
    }
    const day = dateObj.getDate().toString();
    const month = dateObj
      .toLocaleString("es-ES", { month: "short" })
      .toUpperCase()
      .replace(".", "");

    const time = dateObj.toLocaleTimeString("es-ES", {
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });

    return { day, month, time };
  };

  if (loading)
    return <p className="text-gray-500 text-sm">Cargando turnos...</p>;

  if (turnos.length === 0) {
    return (
      <div className="rounded-2xl border border-dashed border-gray-300 p-8 text-center bg-white">
        <p className="text-gray-500">No hay turnos registrados actualmente.</p>
      </div>
    );
  }

  return (
    <div className="flex flex-col gap-4">
      {turnos.map((turno) => {
        const { day, month, time } = formatDate(turno.date);

        return (
          <div
            key={turno.id}
            className="flex items-center justify-between rounded-2xl border border-gray-200 bg-white p-4 shadow-sm transition-all hover:shadow-md"
          >
            <div className="flex items-center gap-4">
              <div className="flex size-16 flex-col items-center justify-center rounded-2xl bg-sky-100 text-sky-900 shrink-0">
                <span className="text-xl font-bold leading-none">{day}</span>
                <span className="text-xs font-semibold uppercase">{month}</span>
                <span className="mt-1 text-xs font-bold text-sky-700 bg-white/70 px-1.5 py-0.5 rounded-md">
                  {time}
                </span>
              </div>

              <div className="flex flex-col gap-1">
                <div className="flex items-center gap-2">
                  <h3 className="font-bold text-gray-900 text-base">
                    {turno.servicioName || turno.servicioId}
                  </h3>
                  <span className="rounded-full bg-amber-100 px-2.5 py-0.5 text-xs font-semibold text-amber-800">
                    Confirmado
                  </span>
                </div>
                <p className="text-sm text-gray-500">
                  {turno.localName || turno.localId}
                </p>
              </div>
            </div>

            <button
              onClick={() => handleCancel(turno.id)}
              className="flex items-center gap-1.5 rounded-xl border border-gray-200 bg-white px-4 py-2 text-sm font-semibold text-gray-700 transition-colors hover:bg-red-50 hover:border-red-200 hover:text-red-600"
            >
              <X size={16} />
              Cancelar
            </button>
          </div>
        );
      })}
    </div>
  );
};
