import React, { useState, useRef } from "react";
import { Link } from "react-router-dom";
import toast from "react-hot-toast";

// interface
import type { UsuarioRegisterDto } from "../../interface/UsuarioType";
import type { HorarioAtencionType } from "../../interface/HorarioAtencionType";
import { DayOfWeek } from "../../interface/HorarioAtencionType";
import type { LocalesType } from "../../interface/LocalesType";

import { addUsuarioLocal } from "../../service/api";
export const RegisterLocal: React.FC = () => {
  const formRef = useRef<HTMLFormElement>(null);
  const [message, setMessage] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const [formError, setFormError] = useState<string | null>(null);

  const handleSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setFormError(null);
    setMessage("");
    setLoading(true);

    const formData = new FormData(e.currentTarget);

    const user: UsuarioRegisterDto = {
      name: formData.get("name") as string,
      email: formData.get("email") as string,
      password: formData.get("password") as string,
      confirmPassword: formData.get("confirmPassword") as string,
      rol: "local",
    };

    if (user.password !== user.confirmPassword) {
      setMessage("❌ Las contraseñas no coinciden");
      setLoading(false);
      return;
    }

    const local: LocalesType = {
      name: formData.get("localName") as string,
      description: formData.get("localDescription") as string,
      category: formData.get("localCategory") as string,
      imageURL: formData.get("localImage") as string,
      direction: formData.get("localAddress") as string,
      phone: formData.get("localPhone") as string,
      servicios: [],
      horariosAtencion: [],
      id: "",
    };

    const diaSelected = formData.get("localDay");
    const diaSemanaNumber =
      diaSelected !== null && diaSelected !== "" ? Number(diaSelected) : -1;

    const horarioAtencion: HorarioAtencionType = {
      localId: "",
      diaSemana: diaSemanaNumber as keyof typeof DayOfWeek,
      horaApertura: formData.get("localOpeningDay") as string,
      horaCierre: formData.get("localClosingDay") as string,
      estaCerrado: false,
    };

    try {
      const rest = await addUsuarioLocal(user, local, horarioAtencion);
      toast.success(rest.message);

      formRef.current?.reset();
    } catch (error: any) {
      setFormError(error.message);
    } finally {
      setLoading(false);
    }
  };

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

        <label>Día de la Semana (0 = Domingo, 1 = Lunes, etc.):</label>
        <select name="localDay" defaultValue="" required>
          <option value="" disabled>
            Elija un dia
          </option>
          <option value="1">Lunes</option>
          <option value="2">Martes</option>
          <option value="3">Miércoles</option>
          <option value="4">Jueves</option>
          <option value="5">Viernes</option>
          <option value="6">Sábado</option>
          <option value="0">Domingo</option>
        </select>

        <label htmlFor="">El dia de apertura</label>
        <input name="localOpeningDay" type="text" />
        <label htmlFor="">El dia de cierre</label>
        <input name="localClosingDay" type="text" />

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
