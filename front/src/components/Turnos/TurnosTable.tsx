import { useEffect, useState } from "react";
import type { TurnosType,TurnosTableProps } from './TurnosType'

import {deleteTurno, getHorarioAtencionUsuario} from '../../service/api'

export const TurnosTable: React.FC<TurnosTableProps> = ({idUsuario}) => {
    const [loading,setLoading] = useState(false);
    const [turnos, setTurnos] = useState<TurnosType[]>([]);

    useEffect (() => {
        const fetchTurnos = async() => {
            setLoading(true);
            try{
                const data = await getHorarioAtencionUsuario(idUsuario);
                console.log("Respuesta backend:", data)
                setTurnos(data);
           }catch (error) {
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
    const confirmacion = window.confirm("¿Estás seguro de que deseas cancelar este turno?");
    if (!confirmacion) return;

    try {
      await deleteTurno(turnoId, idUsuario);
      // Actualizamos el estado filtrando el turno eliminado sin necesidad de recargar la página
      setTurnos((prevTurnos) => prevTurnos.filter((t) => t.id !== turnoId));
      alert("Turno cancelado con éxito.");
    } catch (error: any) {
      alert(error.message || "No se pudo cancelar el turno.");
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
            {turnos.map(turno => (
        <tr key={turno.id}>
            <td>{new Date(turno.date).toLocaleDateString()}</td>
            <td>{turno.localId}</td>
            <td>{turno.servicioId}</td>
            <td>{turno.usuarioId}</td>
             <td>
              <button onClick={() => handleCancelar(turno.id)}>Cancelar turno</button> 
            </td>           
           
        </tr>
       ))}
          </tbody>
       </table>
    )}
    </div>
  );
};

