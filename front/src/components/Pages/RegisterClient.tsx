import React, { useState, useRef } from "react";
import { Link } from "react-router-dom";
import { ArrowRight } from 'lucide-react'
import { addUsuario } from "../../service/api";
import type { UsuarioRegisterDto } from "../../interface/UsuarioType";
import toast from "react-hot-toast";

export const RegisterClient: React.FC = () => {
  const formRef = useRef<HTMLFormElement>(null);
  const [message, setMessage] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const [formError, setFormError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setMessage("");
    setLoading(true);
    setFormError(null);

    const formData = new FormData(e.currentTarget);

    const user: UsuarioRegisterDto = {
      name: formData.get("name") as string,
      email: formData.get("email") as string,
      password: formData.get("password") as string,
      confirmPassword: formData.get("confirmPassword") as string,
      rol: "Cliente",
    };

    if (user.password !== user.confirmPassword) {
      setMessage("❌ Las contraseñas no coinciden");
      setLoading(false);
      return;
    }

    try {
      const res = await addUsuario(user);
      setMessage(res.message);
      toast.success(res.message);

      formRef.current?.reset(); 
    } catch (error: any) {
      setFormError(error.message);
    } finally {
      setLoading(false);
    }
  };

  return (
    <main className="flex min-h-screen items-center justify-center bg-background px-5 py-10">
    <div className="w-full max-w-md rounded-3xl border border-border bg-card p-8 shadow-xl">
      {/* Logo */}
        <Link to="/" className="flex items-center gap-2 text-xl font-semibold">
          <span className="flex size-9 items-center justify-center rounded-xl bg-primary text-sm font-bold text-primary-foreground">
            T
          </span>{" "}
          turnito
        </Link>
        <div className="mt-8">
          <p className="text-sm font-semibold uppercase tracking-wider text-primary">
            Crear cuenta
          </p>
          <h1 className="mt-2 text-3xl font-semibold text-gray-900">Registrate como cliente</h1>
          <p className="mt-2 text-gray-500">Completá tus datos para comenzar.</p>
        </div>

      <form className="mt-8 flex flex-col gap-5"onSubmit={handleSubmit}>
        
        <div className="flex flex-col gap-1.5 text-left">
        <label className="text-sm font-semibold text-gray-800">Nombre</label>
        <input className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black" required name="name" type="text" />
        </div>

        <div className="flex flex-col gap-1.5 text-left">
        <label className="text-sm font-semibold text-gray-800">Email</label>
        <input className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black" required name="email" type="text" />
        </div>

        <div className="flex flex-col gap-1.5 text-left">
        <label className="text-sm font-semibold text-gray-800">Contraseña</label>
        <input className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black" required name="password" type="password" />
        </div>

        <div className="flex flex-col gap-1.5 text-left">
        <label className="text-sm font-semibold text-gray-800">Confirmar Contraseña</label>
        <input className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black" required name="confirmPassword" type="password" />
        </div>
        <button
            type="submit"
            disabled={loading}
            className="mt-2 flex items-center justify-center gap-2 rounded-xl bg-primary px-4 py-3 font-semibold text-primary-foreground transition-all hover:opacity-90 disabled:opacity-50"
          >
            {loading ? "Enviando..." : "Enviar"} <ArrowRight size={17} />
          </button>
      </form>
      {formError && (
        <div style={{ color: "red", marginTop: "15px", fontWeight: "bold" }}>
          ❌ {formError}
        </div>
      )}
      {message && (
          <div className="mt-4 rounded-xl bg-muted p-3 text-center text-sm font-semibold">
            {message}
          </div>
        )}
      
      <p className="mt-6 text-center text-sm text-muted-foreground">
        ¿Ya tenes una cuenta?<Link to="/iniciar-sesion" className="font-semibold text-primary hover:underline">Inicia sesión</Link>
      </p>
    </div>
    </main>
  );
};
