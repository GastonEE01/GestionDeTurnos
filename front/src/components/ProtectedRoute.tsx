import { Navigate, Outlet } from "react-router-dom";
import { useAuthStore } from "../hook/useAuthStore"; // Ajusta la ruta

export const ProtectedRoute = () => {
  const user = useAuthStore((state) => state.user);

  if (!user) {
    return <Navigate to="/iniciar-sesion" replace />;
  }

  return <Outlet />;
};