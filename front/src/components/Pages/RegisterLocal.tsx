import React, { useState,useRef } from "react";
import { Link } from "react-router-dom";
// interface
import type { UsuarioRegisterDto } from "../../interface/UsuarioType";
import type { HorarioAtencionType } from "../../interface/HorarioAtencionType";
import { DayOfWeek } from "../../interface/HorarioAtencionType"; // Sin la palabra 'type'
import type { LocalesType } from "../../interface/LocalesType"; // Sin la palabra 'type'

import { addUsuarioLocal } from "../../service/api";
export const RegisterLocal: React.FC = () => {
      // Estado solo para mostrar el mensaje de existo o error
      const formRef = useRef<HTMLFormElement>(null);
      const [message, setMessage] = useState<string>("");
      const [loading, setLoading] = useState<boolean>(false);

      // Obtener los datos del formulario
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
          rol: "local",
        };

        // Validación express antes de ir al back
      if (user.password !== user.confirmPassword) {
      setMessage("❌ Las contraseñas no coinciden");
      setLoading(false);
      return;
    }
     
    // armamos el objeto local con los datos del formulario
    const local: LocalesType = {
      name: formData.get("localName") as string,
      description: formData.get("localDescription") as string,
        category: formData.get("localCategory") as string,
        imageURL: formData.get("localImage") as string,
      //  title: formData.get("localTitle") as string,
        direction: formData.get("localAddress") as string,
        phone: formData.get("localPhone") as string,
        servicios: [],
        horariosAtencion: [],
        id: "", 
    };

    // aramos el objeto horarioAtencion con los datos del formulario
    const horarioAtencion: HorarioAtencionType = {
      localId: "",
      diaSemana: Number(formData.get("localDay")) as keyof typeof DayOfWeek,
      horaApertura: formData.get("localOpeningDay") as string,
      horaCierre: formData.get("localClosingDay") as string,
      estaCerrado: true,
    };

    try{
        await addUsuarioLocal(user,local,horarioAtencion);
        setMessage("¡Registro exitoso! Ya podés iniciar sesión.");
        formRef.current?.reset(); // Limpia el formulario
    } catch (error) {
        setMessage("No se pudo registrar el local: " + error);
    } finally {
        setLoading(false);
    }
}

  return (
    <div>
      <h1>Registro como Local</h1>
        <form onSubmit={handleSubmit} ref={formRef}>
        <label htmlFor="">Ingrese el nombre</label>
          <input name="name" type="text" />

          <label htmlFor="">Ingrese el gmail</label>
          <input name="email" type="text" />

          <label htmlFor="">Ingrese la contraseña</label>
          <input name="password" type="password" />

          <label htmlFor="">Confirme la contraseña</label>
          <input name="confirmPassword" type="password" />

          <label htmlFor="">Ingrese el nombre del local</label>
          <input name="localName" type="text" />

          <label htmlFor="">Ingrese la descripcion del local</label>
          <input name="localDescription" type="text" />
          
          <label htmlFor="">Ingrese la categoria del local</label>
          <input name="localCategory" type="text" />

          <label htmlFor="">Ingrese una imagen del local</label>
          <input name="localImage" type="text" />

          <label htmlFor="">Ingrese el titulo del local</label>
          <input name="localTitle" type="text" />

          <label htmlFor="">Ingrese la direccion del local</label>
          <input name="localAddress" type="text" />

          <label htmlFor="">Ingrese el telefono del local</label>
          <input name="localPhone" type="text" />

<label htmlFor="">El dia de atencion</label>
          <input name="localDay" type="text" />
<label htmlFor="">El dia de apertura</label>
          <input name="localOpeningDay" type="text" />
<label htmlFor="">El dia de cierre</label>
          <input name="localClosingDay" type="text" />

          
          <button type="submit"  disabled={loading}>Enviar</button>
      </form>
       <h2>
          Ya tenes una cuenta?<Link to="/iniciar-sesion">Logueate</Link>
        </h2>
        {message && <h2>{message}</h2>}
    </div>
  );
}


