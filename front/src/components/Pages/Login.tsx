import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { loginUser } from "../../service/api";
import type { LoginDtoRequest } from "../../interface/LoginType";

export const Login: React.FC = () => {
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();
  const [formError, setFormError] = useState<string | null>(null);

  const handleSutmit = async (e: React.FormEvent<HTMLFormElement>) => {
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
        localStorage.setItem("token", response.token);
        localStorage.setItem(
          "user",
          JSON.stringify({
            id: response.id,
            email: response.email,
            name: response.name,
            rol: response.rol,
          }),
        );
        navigate("/Inicio");
        console.log(response);
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
    <div>
      <h1>Login</h1>

      <form onSubmit={handleSutmit}>
        <label>Ingrese su gmail</label>
        <input name="email" type="text" />
        <label>Ingrese su contraseña</label>
        <input name="password" type="text" />
        <button type="submit" disabled={loading}>
          Ingresar
        </button>
      </form>
      <h2>
        ¿No tiene una cuenta?{" "}
        <Link to="/registro/cliente">Registrate como Cliente</Link>
        ¿No tiene una cuenta?{" "}
        <Link to="/registro/local">Registrate como Local</Link>
      </h2>
      {formError && (
        <div style={{ color: "red", marginTop: "15px", fontWeight: "bold" }}>
          ❌ {formError}
        </div>
      )}
    </div>
  );
};
