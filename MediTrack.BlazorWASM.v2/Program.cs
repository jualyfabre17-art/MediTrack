using MediTrack.BlazorWASM.v2;
using MediTrack.BlazorWASM.v2.Services;
using MediTrack.Web.V2.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Configurar HttpClient para la API
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7071/") // ← Puerto de tu API
});

// Registrar servicios del frontend
builder.Services.AddScoped<IPatientService, PatientService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
// Registramos nuestro servicio centralizado de API
builder.Services.AddScoped<ApiService>();

await builder.Build().RunAsync();