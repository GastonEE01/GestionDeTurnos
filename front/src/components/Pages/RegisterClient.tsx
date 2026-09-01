import React, { useState, useRef } from "react";
import { Link } from "react-router-dom";
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
    <div>
      <h1>Registro como cliente</h1>
      <form onSubmit={handleSubmit}>
        <label htmlFor="">Ingrese el nombre</label>
        <input name="name" type="text" />

        <label htmlFor="">Ingrese el gmail</label>
        <input name="email" type="text" />

        <label htmlFor="">Ingrese la contraseña</label>
        <input name="password" type="password" />

        <label htmlFor="">Confirme la contraseña</label>
        <input name="confirmPassword" type="password" />

        <button type="submit" disabled={loading}>
          Enviar
        </button>
      </form>
      {formError && (
        <div style={{ color: "red", marginTop: "15px", fontWeight: "bold" }}>
          ❌ {formError}
        </div>
      )}
      <h2>
        Ya tenes una cuenta?<Link to="/iniciar-sesion">Logueate</Link>
      </h2>
      {message && <h2>{message}</h2>}
    </div>
  );
};
