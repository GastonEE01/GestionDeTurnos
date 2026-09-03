import { Navigate, Outlet } from "react-router-dom";
import { useAuthStore } from "../hook/useAuthStore";

export const ProtectedRoute = () => {
  const user = useAuthStore((state) => state.user);
  if (!user) {
    return <Navigate to="/iniciar-sesion" replace />;
  }

  return <Outlet />;
};