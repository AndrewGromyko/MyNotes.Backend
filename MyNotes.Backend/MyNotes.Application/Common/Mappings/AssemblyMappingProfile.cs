﻿using AutoMapper;
using MyNotes.Domain.Interfaces.Services;
using System.Reflection;

namespace MyNotes.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile(Assembly assembly) =>
            ApplyMappingsFromFromAssembly(assembly);

        private void ApplyMappingsFromFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.GetInterfaces()
                    .Any(i => i.IsGenericType &&
                    i.GetGenericTypeDefinition() == typeof(IMapWith<>)))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                var methodInfo = type.GetMethod("Mapping");
                methodInfo?.Invoke(instance, new object[] { this });
            }
        }
    }
}
