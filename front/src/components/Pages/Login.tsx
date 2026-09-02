import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { loginUser } from "../../service/api";
import type { LoginDtoRequest } from "../../interface/LoginType";
import { ArrowRight } from "lucide-react";
import { useAuthStore } from "../../hook/useAuthStore";

export const Login: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();
  const [formError, setFormError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setLoading(true);
    setFormError(null);

    const formData = new FormData(e.currentTarget);

    // Armar el objeto
    const userLogin: LoginDtoRequest = {
      email: formData.get("email") as string,
      password: formData.get("password") as string,
    };

    try {
      const response = await loginUser(userLogin);
      if (response) {
        useAuthStore.getState().login(
          {
          
            id: response.id,
            email: response.email,
            name: response.name,
            rol: response.rol,
            token: response.token,
          },
         response.token);
        navigate("/Inicio");
      }

      e.currentTarget.reset();
    } catch (error: unknown) {
      if (error instanceof Error) setFormError(error.message);
      else setFormError("Hubo un error inesperado al inicial sesion");
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="flex min-h-screen w-full items-center justify-center bg-background px-4 py-8">
      <div className="w-full max-w-md rounded-3xl border border-border bg-card p-6 sm:p-8 shadow-xl">
        <div className="flex items-center gap-2 text-xl font-semibold">
          <span className="flex size-9 items-center justify-center rounded-xl bg-primary text-sm font-bold text-primary-foreground">
            T
          </span>
          turnito
        </div>

        <div className="mt-8">
          <p className="text-xs sm:text-sm font-semibold uppercase tracking-wider text-primary">
            Bienvenido de nuevo
          </p>
          <h1 className="mt-2 text-2xl sm:text-3xl font-semibold text-foreground">
            Iniciá sesión
          </h1>
          <p className="mt-2 text-sm text-muted-foreground">
            Accedé a tus turnos o administrá tu local.
          </p>
        </div>

        <form className="mt-6 flex flex-col gap-4" onSubmit={handleSubmit}>
          <label className="flex flex-col gap-1.5 text-sm font-semibold text-foreground">
            Ingrese su gmail
          </label>
          <input
            className="h-11 rounded-xl border border-input bg-background px-4 font-normal text-foreground outline-none focus:ring-2 focus:ring-ring"
            name="email"
            type="email"
            placeholder="vos@ejemplo.com"
            required
          />
          <label className="flex flex-col gap-1.5 text-sm font-semibold text-foreground">
            Ingrese su contraseña
          </label>
          <input
            className="h-11 rounded-xl border border-input bg-background px-4 font-normal text-foreground outline-none focus:ring-2 focus:ring-ring"
            name="password"
            type="password"
            placeholder="••••••••"
            required
          />
          <button
            className="mt-2 flex h-12 items-center justify-center gap-2 rounded-xl bg-primary px-4 font-semibold text-primary-foreground transition-all hover:opacity-90"
            type="submit"
            disabled={loading}
          >
            Ingresar <ArrowRight size={17} />
          </button>
        </form>
        {formError && (
          <div style={{ color: "red", marginTop: "15px", fontWeight: "bold" }}>
            ❌ {formError}
          </div>
        )}
        <p className="mt-6 text-center text-sm text-muted-foreground">
          ¿Todavía no tenés cuenta?{" "}
          <Link to="/" className="font-semibold text-black hover:underline">
            Volver al inicio
          </Link>
        </p>
      </div>
    </main>
  );
};
