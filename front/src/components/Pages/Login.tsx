import React, { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import {loginUser} from '../../service/api'

export interface LoginType {
  email: string;
  password: string;
}

export const Login: React.FC = () => {
  const [message, setMessage] = useState<string>("",);
  const [loading, setLoading] = useState<boolean>(false);
  const navigate = useNavigate();

  const handleSutmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setMessage("");
    setLoading(true);

    // Capturar los datos del formulario
    const formData = new FormData(e.currentTarget);

    // Armar el objeto
    const userLogin: LoginType = {
      email: formData.get("email") as string,
      password: formData.get("password") as string,
    };

    // Opcional aca puedo hacer alguna validacion antes de mandarlo al back
    try {
      const response = await loginUser(userLogin);
      if(response ){
        localStorage.setItem("token", response.token);
        localStorage.setItem("user", JSON.stringify(response.response));
        navigate("/Home")
        console.log(response);
      }
     //  console.log("Token recibido:", response.token);
      e.currentTarget.reset();
    } catch (error) {
 if (error instanceof Error) {
    setMessage(`Hubo un error al iniciar sesión: ${error.message}`);
  } else {
    setMessage("Hubo un error inesperado al iniciar sesión.");
  }
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
        <button type="submit" disabled={loading}>Ingresar</button>
      </form>
      <h2>
        ¿No tiene una cuenta? <Link to="/Registro">Registrate</Link>
      </h2>
      {message && <h2>{message}</h2>}
    </div>
  );
};

