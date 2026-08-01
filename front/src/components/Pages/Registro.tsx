import React, { useState,useRef } from "react";
import { Link } from "react-router-dom";
import { addUsuario } from "../../service/api";
// 1) Definimos el molde del objeto
export interface UsuarioType {
  id: number;
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
  rol: string;
}

export interface UsuarioRegisterDto {
  name: string;
  email: string;
  password: string;
  confirmPassword: string;
  rol: string;
}

// Como es una página que carga sola, no requiere Props externas

// 3) Inyectar en el componente
export const Registro: React.FC = () => {
  // Estado solo para mostrar el mensaje de existo o error
  const formRef = useRef<HTMLFormElement>(null);
  const [message, setMessage] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);

  // capturamos los datos del formulario
  //const formData = new FormData(e.currentTarget);
  // const selectedRol = String(formData.get('rol'));

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setMessage("");
    setLoading(true);

    // 3) Capturamos los datos del formulario de manera nativa
    const formData = new FormData(e.currentTarget);

    // 4) Armamos el objeto EXACTAMENTE como lo espera tu backend en C#
    const user: UsuarioRegisterDto = {
      name: formData.get("name") as string,
      email: formData.get("email") as string,
      password: formData.get("password") as string,
      confirmPassword: formData.get("confirmPassword") as string,
      rol: formData.get("rol") as string,
    };

    // Validación express antes de ir al back
    if (user.password !== user.confirmPassword) {
      setMessage("❌ Las contraseñas no coinciden");
      setLoading(false);
      return;
    }

    try {
      await addUsuario(user);
      setMessage("¡Registro exitoso! Ya podés iniciar sesión.");
      formRef.current?.reset(); // Limpia el formulario
    } catch (error) {
      console.error("No se pudo registrar al usuario: ", error);
      setMessage("❌ Hubo un error al registrar el usuario.");
    } finally {
      setLoading(false);
    }
  };
  /*
     const handleChange = (e) => {
            const {name, value} = e.target;
            const {gmail, value} = e.target;
            const {password, value} = e.target;
            const {rol, value} = e.target;

            setFormData(prev => ({
                ...prev,
                [name]: value
            }));
        };*/

 
    return (
      <div>
        <h1>Registro</h1>
        <form onSubmit={handleSubmit}>
          <label htmlFor="">Ingrese el nombre</label>
          <input name="name" type="text" />

          <label htmlFor="">Ingrese el gmail</label>
          <input name="email" type="text" />

          <label htmlFor="">Ingrese la contraseña</label>
          <input name="password" type="password" />

          <label htmlFor="">Confirme la contraseña</label>
          <input name="confirmPassword" type="password" />

          <label htmlFor="">Elija su rol</label>
          <select name="rol" id="rol">
            <option value="Cliente">Cliente</option>
            <option value="Local">Local</option>
          </select>
          <button type="submit"  disabled={loading}>Enviar</button>
        </form>
        <h2>
          Ya tenes una cuenta?<Link to="/">Logueate</Link>
        </h2>
        {message && <h2>{message}</h2>}
      </div>
    );
  }
