import {useState}  from 'react'
import { ArrowRight, Calendar, MapPin, Sparkles, Store, Zap, Clock } from 'lucide-react'
import { Header } from '../Header/Header.tsx' 

export const Landing = () => {
      const [activeTab, setActiveTab] = useState<'client' | 'business'>('client')

  return (
    <div className="min-h-screen bg-gradient-to-br from-background via-background to-secondary/30">
      <Header />
      

      {/* Hero Section */}
      <section className="relative mx-auto max-w-7xl px-5 py-20 lg:px-8">
        <div className="grid gap-12 lg:grid-cols-2 lg:items-center">
          <div className="flex flex-col gap-6">
            <div className="flex items-center gap-2">
              <div className="flex size-10 items-center justify-center rounded-lg bg-accent/20">
                <Sparkles size={20} className="text-accent" />
              </div>
              <span className="text-sm font-semibold uppercase tracking-wider text-accent">Simplificar turnos</span>
            </div>
            
            <h1 className="text-5xl font-bold leading-tight tracking-tight text-foreground md:text-6xl">
              Reservá turnos en segundos.{' '}
              <span className="bg-gradient-to-r from-primary to-accent bg-clip-text text-transparent">
                Sin complicaciones.
              </span>
            </h1>
            
            <p className="text-xl leading-relaxed text-muted-foreground max-w-lg">
              turnito conecta clientes con locales de servicios. Reserva, cancela y gestiona todo desde tu bolsillo.
            </p>

            <div className="flex flex-col gap-3 sm:flex-row sm:items-center">
              <button className="group flex items-center justify-center gap-2 rounded-xl bg-primary px-6 py-3.5 font-semibold text-primary-foreground transition-all hover:shadow-xl hover:shadow-primary/30 hover:-translate-y-0.5">
                Empezar ahora <ArrowRight size={18} className="group-hover:translate-x-1 transition-transform" />
              </button>
              <button className="flex items-center justify-center gap-2 rounded-xl border border-border px-6 py-3.5 font-semibold text-foreground transition-colors hover:bg-muted">
                Ver demo
              </button>
            </div>

            <div className="flex items-center gap-4 pt-2 text-sm text-muted-foreground">
              <div className="flex -space-x-2">
                {[...Array(3)].map((_, i) => (
                  <div key={i} className="flex size-8 items-center justify-center rounded-full border-2 border-background bg-gradient-to-br from-primary/30 to-accent/30 text-xs font-semibold">
                    {String.fromCharCode(65 + i)}
                  </div>
                ))}
              </div>
              <span>Más de 500 locales confían en turnito</span>
            </div>
          </div>

          {/* Visual Preview */}
          <div className="relative">
            <div className="rounded-3xl border border-border/50 bg-card p-6 shadow-2xl overflow-hidden">
              <div className="grid grid-cols-2 gap-3">
                {[
                  { icon: Calendar, label: 'Reservá turnos', desc: 'En minutos' },
                  { icon: Clock, label: 'Sin esperas', desc: 'Confirmado' },
                  { icon: MapPin, label: 'Cerca tuyo', desc: '5 locales' },
                  { icon: Zap, label: 'Acceso fácil', desc: 'Desde el celular' },
                ].map((item, i) => (
                  <div key={i} className="rounded-xl border border-border/50 bg-muted/30 p-4 backdrop-blur-sm">
                    <div className="flex size-10 items-center justify-center rounded-lg bg-accent/20 mb-2">
                      <item.icon size={20} className="text-accent" />
                    </div>
                    <p className="text-sm font-semibold">{item.label}</p>
                    <p className="text-xs text-muted-foreground">{item.desc}</p>
                  </div>
                ))}
              </div>
            </div>
            <div className="absolute -bottom-8 -right-8 size-32 rounded-full bg-accent/10 blur-3xl" aria-hidden="true" />
          </div>
        </div>
      </section>

      {/* Two Paths Section */}
      <section className="mx-auto max-w-7xl px-5 py-20 lg:px-8">
        <div className="mb-12 flex flex-col items-center text-center gap-4">
          <p className="text-sm font-semibold uppercase tracking-wider text-primary">Para cada tipo de usuario</p>
          <h2 className="text-4xl font-bold tracking-tight text-foreground md:text-5xl">¿Cómo usarás turnito?</h2>
          <p className="text-lg text-muted-foreground max-w-2xl">Tengas un local o busques un servicio, turnito tiene lo que necesitás.</p>
        </div>

        <div className="grid gap-8 lg:grid-cols-2">
          {/* Client Path */}
          <div
            onClick={() => setActiveTab('client')}
            className={`group cursor-pointer rounded-3xl border-2 transition-all p-8 ${
              activeTab === 'client'
                ? 'border-primary bg-gradient-to-br from-primary/10 to-background shadow-xl'
                : 'border-border/30 hover:border-border'
            }`}
          >
            <div className="flex size-14 items-center justify-center rounded-2xl bg-primary/20 mb-6 group-hover:scale-110 transition-transform">
              <Calendar size={28} className="text-primary" />
            </div>
            <h3 className="text-2xl font-bold mb-3 text-foreground">Soy cliente</h3>
            <p className="text-muted-foreground mb-6 leading-relaxed">
              Reserva turnos en tus lugares favoritos, recibe confirmación instantánea y mantenné tus turnos siempre a mano.
            </p>
            <ul className="space-y-3 mb-8">
              {[
                'Descubrí nuevos locales cerca tuyo',
                'Reserva sin llamadas telefónicas',
                'Recibí recordatorios automáticos',
                'Cancelá cuando lo necesites',
              ].map((item, i) => (
                <li key={i} className="flex items-start gap-3">
                  <div className="flex size-5 shrink-0 items-center justify-center rounded-full bg-primary/20 mt-0.5">
                    <ArrowRight size={14} className="text-primary" />
                  </div>
                  <span className="text-sm">{item}</span>
                </li>
              ))}
            </ul>
            <a href="/registro/cliente" className="block w-full rounded-xl bg-primary px-4 py-3 text-center font-semibold text-primary-foreground transition-all hover:shadow-lg hover:shadow-primary/30">
              Registrarse como cliente
            </a>
          </div>

          {/* Business Path */}
          <div
            onClick={() => setActiveTab('business')}
            className={`group cursor-pointer rounded-3xl border-2 transition-all p-8 ${
              activeTab === 'business'
                ? 'border-accent bg-gradient-to-br from-accent/10 to-background shadow-xl'
                : 'border-border/30 hover:border-border'
            }`}
          >
            <div className="flex size-14 items-center justify-center rounded-2xl bg-accent/20 mb-6 group-hover:scale-110 transition-transform">
              <Store size={28} className="text-accent" />
            </div>
            <h3 className="text-2xl font-bold mb-3 text-foreground">Soy local</h3>
            <p className="text-muted-foreground mb-6 leading-relaxed">
              Administra tus locales, servicios, horarios y turnos en un solo lugar. Aumentá tus clientes y reduce no-shows.
            </p>
            <ul className="space-y-3 mb-8">
              {[
                'Gestiona múltiples locales',
                'Define tus servicios y precios',
                'Configura horarios flexibles',
                'Reduce cancelaciones',
              ].map((item, i) => (
                <li key={i} className="flex items-start gap-3">
                  <div className="flex size-5 shrink-0 items-center justify-center rounded-full bg-accent/20 mt-0.5">
                    <ArrowRight size={14} className="text-accent" />
                  </div>
                  <span className="text-sm">{item}</span>
                </li>
              ))}
            </ul>
            <a href="/registro/local" className="block w-full rounded-xl bg-accent px-4 py-3 text-center font-semibold text-accent-foreground transition-all hover:shadow-lg hover:shadow-accent/30">
              Registrarse como local
            </a>
          </div>
        </div>
      </section>

      {/* Features Section */}
      <section className="mx-auto max-w-7xl px-5 py-20 lg:px-8">
        <div className="mb-12 flex flex-col items-center text-center gap-4">
          <p className="text-sm font-semibold uppercase tracking-wider text-primary">¿Por qué turnito?</p>
          <h2 className="text-4xl font-bold tracking-tight text-foreground md:text-5xl">Todo lo que necesitás</h2>
        </div>

        <div className="grid gap-6 md:grid-cols-3">
          {[
            { icon: Zap, title: 'Súper rápido', desc: 'Reserva en menos de 10 segundos sin formularios largos.' },
            { icon: Clock, title: 'Disponibilidad en vivo', desc: 'Ve horarios reales y elige el que se adapte a vos.' },
            { icon: MapPin, title: 'Locales cerca tuyo', desc: 'Descubrí nuevos servicios en tu zona.' },
            { icon: Calendar, title: 'Gestión sencilla', desc: 'Panel de control claro para administrar todo.' },
            { icon: Store, title: 'Para múltiples locales', desc: 'Maneja todas tus sucursales desde un solo sitio.' },
            { icon: Sparkles, title: 'Recordatorios automáticos', desc: 'Recibí notificaciones sin perder un turno.' },
          ].map((feature, i) => (
            <div key={i} className="rounded-2xl border border-border/50 bg-card p-6 hover:shadow-lg hover:border-primary/50 transition-all">
              <div className="flex size-12 items-center justify-center rounded-xl bg-primary/20 mb-4">
                <feature.icon size={24} className="text-primary" />
              </div>
              <h3 className="font-semibold mb-2 text-foreground">{feature.title}</h3>
              <p className="text-sm text-muted-foreground">{feature.desc}</p>
            </div>
          ))}
        </div>
      </section>

      {/* CTA Section */}
      <section className="mx-auto max-w-7xl px-5 py-20 lg:px-8">
        <div className="rounded-3xl border border-primary/30 bg-gradient-to-r from-primary/10 to-accent/10 p-12 text-center">
          <h2 className="text-4xl font-bold mb-4 text-foreground">¿Listo para empezar?</h2>
          <p className="text-lg text-muted-foreground mb-8 max-w-2xl mx-auto">
            Únete a cientos de usuarios que ya están simplificando sus turnos.
          </p>
          <div className="flex flex-col gap-3 sm:flex-row sm:justify-center sm:items-center">
            <a href="/registro/cliente" className="rounded-xl bg-primary px-8 py-3.5 text-center font-semibold text-primary-foreground transition-all hover:shadow-lg hover:shadow-primary/30 hover:-translate-y-0.5">
              Registrarse como cliente
            </a>
            <a href="/registro/local" className="rounded-xl bg-accent px-8 py-3.5 text-center font-semibold text-accent-foreground transition-all hover:shadow-lg hover:shadow-accent/30 hover:-translate-y-0.5">
              Registrarse como local
            </a>
          </div>
        </div>
      </section>

      {/* Footer */}
      <footer className="border-t border-border/50 bg-card/50 backdrop-blur-sm mt-20">
        <div className="mx-auto max-w-7xl px-5 py-12 lg:px-8">
          <div className="grid gap-8 md:grid-cols-4">
            <div>
              <div className="flex items-center gap-2 mb-4">
                <div className="flex size-8 items-center justify-center rounded-lg bg-primary text-xs font-bold text-primary-foreground">
                  T
                </div>
                <span className="font-semibold">turnito</span>
              </div>
              <p className="text-sm text-muted-foreground">Simplificando turnos en Argentina.</p>
            </div>
            <div>
              <p className="text-sm font-semibold mb-3">Producto</p>
              <ul className="space-y-2 text-sm text-muted-foreground">
                <li><a href="#" className="hover:text-foreground transition-colors">Para clientes</a></li>
                <li><a href="#" className="hover:text-foreground transition-colors">Para locales</a></li>
                <li><a href="#" className="hover:text-foreground transition-colors">Precios</a></li>
              </ul>
            </div>
            <div>
              <p className="text-sm font-semibold mb-3">Empresa</p>
              <ul className="space-y-2 text-sm text-muted-foreground">
                <li><a href="#" className="hover:text-foreground transition-colors">Sobre nosotros</a></li>
                <li><a href="#" className="hover:text-foreground transition-colors">Blog</a></li>
                <li><a href="#" className="hover:text-foreground transition-colors">Contacto</a></li>
              </ul>
            </div>
            <div>
              <p className="text-sm font-semibold mb-3">Legal</p>
              <ul className="space-y-2 text-sm text-muted-foreground">
                <li><a href="#" className="hover:text-foreground transition-colors">Privacidad</a></li>
                <li><a href="#" className="hover:text-foreground transition-colors">Términos</a></li>
              </ul>
            </div>
          </div>
          <div className="mt-8 border-t border-border/50 pt-8 text-center text-sm text-muted-foreground">
            <p>&copy; 2026 turnito. Todos los derechos reservados.</p>
          </div>
        </div>
      </footer>
    </div>
  )
}
