import React, { useState, useRef } from "react";
import { Link } from "react-router-dom";
import toast from "react-hot-toast";
import { ArrowRight,Trash2,Plus } from "lucide-react";

// interface
import type { UsuarioRegisterDto } from "../../interface/UsuarioType";
import type { HorarioAtencionType } from "../../interface/HorarioAtencionType";
import { DayOfWeek } from "../../interface/HorarioAtencionType";
import type { LocalesType } from "../../interface/LocalesType";
import { addUsuarioLocal } from "../../service/api";

const CATEGORIAS_LOCALES = [
  "Peluquería y Barbería",
  "Estética y Spa",
  "Canchas y Deportes",
  "Salud y Odontología",
  "Gastronomía y Bares",
  "Indumentaria y Ropa",
  "Veterinaria y Mascotas",
  "Taller Mecánico",
  "Talleres y Clases",
  "Otros Servicios"
];

const DIAS_SEMANA = [
  { label: "Lunes", value: 1 },
  { label: "Martes", value: 2 },
  { label: "Miércoles", value: 3 },
  { label: "Jueves", value: 4 },
  { label: "Viernes", value: 5 },
  { label: "Sábado", value: 6 },
  { label: "Domingo", value: 0 },
];

export const RegisterLocal: React.FC = () => {
  const formRef = useRef<HTMLFormElement>(null);
  const [message, setMessage] = useState<string>("");
  const [loading, setLoading] = useState<boolean>(false);
  const [formError, setFormError] = useState<string | null>(null);

  const [horariosList, setHorariosList] = useState<HorarioAtencionType[]>([]);
  const [selectedDay, setSelectedDay] = useState<string>("");
  const [openingTime, setOpeningTime] = useState<string>("09:00");
  const [closingTime, setClosingTime] = useState<string>("18:00");


  const handleAddHorario = () => {
    if (selectedDay === "") {
      toast.error("Selecciona un día para agregar");
      return;
    }

    const diaNumber = Number(selectedDay);

    // Evitar duplicar el mismo día
    if (horariosList.some((h) => Number(h.diaSemana) === diaNumber)) {
      toast.error("Ya agregaste ese día. Elimínalo si deseas cambiar el horario.");
      return;
    }

    const nuevoHorario: HorarioAtencionType = {
      localId: "",
      diaSemana: diaNumber as keyof typeof DayOfWeek,
      horaApertura: openingTime,
      horaCierre: closingTime,
      estaCerrado: false,
    };

    setHorariosList([...horariosList, nuevoHorario]);
    setSelectedDay(""); // Resetea el selector de día
  };

  const handleRemoveHorario = (index: number) => {
    setHorariosList(horariosList.filter((_, i) => i !== index));
  };
  
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

    if (horariosList.length === 0) {
      toast.error("Debes agregar al menos un horario de atención");
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
      horariosAtencion: horariosList,
      id: "",
    };

    /*const diaSelected = formData.get("localDay");
    const diaSemanaNumber =
      diaSelected !== null && diaSelected !== "" ? Number(diaSelected) : -1;

    const horarioAtencion: HorarioAtencionType = {
      localId: "",
      diaSemana: diaSemanaNumber as keyof typeof DayOfWeek,
      horaApertura: formData.get("localOpeningDay") as string,
      horaCierre: formData.get("localClosingDay") as string,
      estaCerrado: false,
    };*/

    try {
      const rest = await addUsuarioLocal(user, local, horariosList);
      toast.success(rest.message);

      formRef.current?.reset();
    } catch (error: unknown) {
      const errorObject = error as Error;
      setFormError(errorObject.message);
    } finally {
      setLoading(false);
    }
  };

return (
    <main className="flex min-h-screen items-center justify-center bg-background px-5 py-10">
      {/* 1. Ampliado a max-w-4xl para que entren cómodamente las 2 columnas */}
      <div className="w-full max-w-4xl rounded-3xl border border-border bg-card p-8 shadow-xl">
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
          <h1 className="mt-2 text-3xl font-semibold text-gray-900">
            Registrate como local
          </h1>
          <p className="mt-2 text-gray-500">
            Completá los datos de tu cuenta y de tu negocio.
          </p>
        </div>

        <form onSubmit={handleSubmit} ref={formRef} className="space-y-6 mt-6">
          {/* GRID CONTENEDOR EN 2 COLUMNAS */}
          <div className="grid grid-cols-1 md:grid-cols-2 gap-6">

            {/* COLUMNA 1: DATOS DE USUARIO */}
            <div className="flex flex-col gap-4">
              <h2 className="text-base font-semibold text-gray-900 border-b border-gray-200 pb-2 text-left">
                Datos de la Cuenta
              </h2>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Nombre
                </label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  required
                  name="name"
                  type="text"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">Email</label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  required
                  name="email"
                  type="email"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Contraseña
                </label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  required
                  name="password"
                  type="password"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Confirmar Contraseña
                </label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  required
                  name="confirmPassword"
                  type="password"
                />
              </div>
            </div>

            {/* COLUMNA 2: DATOS DEL LOCAL */}
            <div className="flex flex-col gap-4">
              <h2 className="text-base font-semibold text-gray-900 border-b border-gray-200 pb-2 text-left">
                Información del Local
              </h2>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Nombre del Local
                </label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  required
                  name="localName"
                  type="text"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Categoría
                </label>
                <select
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  required
                  name="localCategory"
                  defaultValue=""
                >
                  <option value="" disabled>
                    Selecciona una categoría
                  </option>
                  {CATEGORIAS_LOCALES.map((cat) => (
                    <option key={cat} value={cat}>
                      {cat}
                    </option>
                  ))}
                </select>
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Descripción
                </label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  required
                  name="localDescription"
                  type="text"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Dirección
                </label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  name="localAddress"
                  type="text"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">
                  Teléfono
                </label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  name="localPhone"
                  type="text"
                />
              </div>

              <div className="flex flex-col gap-1.5 text-left">
                <label className="text-sm font-semibold text-gray-800">Imagen (URL)</label>
                <input
                  className="w-full rounded-xl border border-gray-300 bg-white px-4 py-2.5 font-normal text-gray-900 outline-none focus:border-black focus:ring-2 focus:ring-black"
                  name="localImage"
                  type="text"
                />
              </div>
            </div>

          </div> {/* FIN DEL GRID DE 2 COLUMNAS */}

          {/* SECCIÓN MÚLTIPLES HORARIOS (Ocupa el ancho completo abajo) */}
          <div className="rounded-2xl border border-gray-200 bg-gray-50 p-5 text-left">
            <h3 className="text-base font-semibold text-gray-900 mb-1">
              Horarios de Atención
            </h3>
            <p className="text-xs text-gray-500 mb-4">
              Agregá los días y horarios en los que estará abierto tu local.
            </p>

            <div className="grid grid-cols-1 sm:grid-cols-3 gap-3 items-end mb-4">
              <div>
                <label className="text-xs font-semibold text-gray-700">Día</label>
                <select
                  value={selectedDay}
                  onChange={(e) => setSelectedDay(e.target.value)}
                  className="w-full rounded-xl border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 outline-none"
                >
                  <option value="" disabled>Elegí un día</option>
                  {DIAS_SEMANA.map((d) => (
                    <option key={d.value} value={d.value}>
                      {d.label}
                    </option>
                  ))}
                </select>
              </div>

              <div>
                <label className="text-xs font-semibold text-gray-700">Apertura</label>
                <input
                  type="time"
                  value={openingTime}
                  onChange={(e) => setOpeningTime(e.target.value)}
                  className="w-full rounded-xl border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 outline-none"
                />
              </div>

              <div>
                <label className="text-xs font-semibold text-gray-700">Cierre</label>
                <input
                  type="time"
                  value={closingTime}
                  onChange={(e) => setClosingTime(e.target.value)}
                  className="w-full rounded-xl border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 outline-none"
                />
              </div>
            </div>

            <button
              type="button"
              onClick={handleAddHorario}
              className="flex items-center gap-2 rounded-xl bg-slate-900 px-4 py-2 text-xs font-semibold text-white transition-all hover:bg-slate-800"
            >
              <Plus size={16} /> Agregar horario
            </button>

            {/* LISTADO DE DÍAS AGREGADOS */}
            {horariosList.length > 0 && (
              <div className="mt-4 space-y-2 border-t border-gray-200 pt-3">
                <p className="text-xs font-bold text-gray-700 uppercase tracking-wider">
                  Días cargados ({horariosList.length}):
                </p>
                <div className="grid grid-cols-1 sm:grid-cols-2 gap-2">
                  {horariosList.map((item, index) => {
                    const nombreDia = DIAS_SEMANA.find(
                      (d) => d.value === Number(item.diaSemana)
                    )?.label;
                    return (
                      <div
                        key={index}
                        className="flex items-center justify-between rounded-xl border border-gray-200 bg-white px-3 py-2 text-sm"
                      >
                        <span className="font-medium text-gray-800">
                          {nombreDia}: {item.horaApertura} a {item.horaCierre}
                        </span>
                        <button
                          type="button"
                          onClick={() => handleRemoveHorario(index)}
                          className="text-red-500 hover:text-red-700 transition-colors"
                        >
                          <Trash2 size={16} />
                        </button>
                      </div>
                    );
                  })}
                </div>
              </div>
            )}
          </div>

          <button
            type="submit"
            disabled={loading}
            className="w-full flex items-center justify-center gap-2 rounded-xl bg-primary px-4 py-3.5 font-semibold text-primary-foreground transition-all hover:opacity-90 disabled:opacity-50"
          >
            {loading ? "Enviando..." : "Registrar Local"} <ArrowRight size={17} />
          </button>
        </form>

        {formError && (
          <div className="mt-4 rounded-xl bg-red-50 p-3 text-center text-sm font-semibold text-red-600">
            ❌ {formError}
          </div>
        )}
        
        {message && (
          <div className="mt-4 rounded-xl bg-muted p-3 text-center text-sm font-semibold">
            {message}
          </div>
        )}

        <p className="mt-6 text-center text-sm text-muted-foreground">
          ¿Ya tenés una cuenta?{" "}
          <Link
            to="/iniciar-sesion"
            className="font-semibold text-primary hover:underline"
          >
            Iniciá sesión
          </Link>
        </p>
      </div>
    </main>
  );
};