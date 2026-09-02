import { Link } from "react-router-dom";

export const Header = () => {
  return (
    <header className="border-b border-border/50 bg-card/80 backdrop-blur-sm sticky top-0 z-40">
      <div className="mx-auto flex max-w-7xl items-center justify-between px-5 py-4 lg:px-8">
        {/* Logo */}
        <Link to="/" className="flex items-center gap-3">
          <div className="flex size-10 items-center justify-center rounded-xl bg-gradient-to-br from-primary to-accent text-sm font-bold text-primary-foreground shadow-lg">
            T
          </div>
          <span className="text-2xl font-semibold tracking-tight bg-gradient-to-r from-primary to-accent bg-clip-text text-transparent">
            turnito
          </span>
        </Link>

        {/* Acciones del Header */}
        <div className="flex items-center gap-3">
          <Link
            to="/iniciar-sesion"
            className="rounded-lg px-4 py-2 text-sm font-semibold text-foreground transition-colors hover:bg-muted"
          >
            Iniciar sesión
          </Link>

          {/* Menú Desplegable de Registro */}
          <div className="relative group">
            <button className="rounded-lg bg-primary px-4 py-2 text-sm font-semibold text-primary-foreground transition-all hover:shadow-lg hover:shadow-primary/20">
              Registrarse
            </button>

            <div className="absolute right-0 top-full mt-1 hidden w-44 rounded-xl border border-border/80 bg-card p-1.5 shadow-xl group-hover:block z-50">
              <Link
                to="/registro/cliente"
                className="block rounded-lg px-3 py-2 text-sm font-medium text-foreground hover:bg-muted"
              >
                Como cliente
              </Link>
              <Link
                to="/registro/local"
                className="block rounded-lg px-3 py-2 text-sm font-medium text-foreground hover:bg-muted"
              >
                Como local
              </Link>
            </div>
          </div>
        </div>
      </div>
    </header>
  );
};
