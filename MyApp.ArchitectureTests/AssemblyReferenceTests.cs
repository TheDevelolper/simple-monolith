using System.Reflection;
using Data;
using Data.Contracts;
using Domain;
using Domain.Contracts;
using MyApp.Presentation;
using MyApp.Shared;
using NetArchTest.Rules;

namespace MyApp.ArchitectureTests;

public class AssemblyReferenceTests
{
    private readonly string?[] _presentationNameSpaces;
    private readonly string?[] _domainNamespaces;
    private readonly string?[] _dataNamespaces;
    
    private readonly Assembly _presentationAssembly;
    private readonly Assembly _domainAssembly;
    private readonly Assembly _dataAssembly;
    private readonly Assembly _sharedAssembly;

    public AssemblyReferenceTests()
    {
        _presentationAssembly = typeof(IPresentationMarker).Assembly;
        _domainAssembly = typeof(IDomainMarker).Assembly;
        _dataAssembly = typeof(IDataMarker).Assembly;
        _sharedAssembly = typeof(ISharedMarker).Assembly;
        
        _presentationNameSpaces = 
            typeof(IPresentationMarker).Assembly.DefinedTypes.Select(t => t.Namespace)
                .Where(name => name is not null).ToArray();
        
        _domainNamespaces = 
            typeof(IDomainMarker).Assembly.DefinedTypes.Select(t => t.Namespace)
                .Where(name => name is not null).ToArray();
        
        _dataNamespaces = 
            typeof(IDataMarker).Assembly.DefinedTypes.Select(t => t.Namespace)
                .Where(name => name is not null).ToArray();
   

    }
    
    [Fact]
    public void DomainServicesDoNotReferenceControllers()
    {
        var result = 
            Types.InAssembly(_domainAssembly) 
                .That().ImplementInterface(typeof(IProductService))
            .ShouldNot()
            .HaveDependencyOnAny(_presentationNameSpaces)
            .GetResult();

        Assert.True(result.IsSuccessful);
    }
    
    [Fact]
    public void DataServicesDoNotReferenceControllers()
    {
        var result = 
            Types.InAssembly(_domainAssembly) 
                .That().ImplementInterface(typeof(IRepository))
                .ShouldNot()
                .HaveDependencyOnAny(_presentationNameSpaces)
                .GetResult();

        Assert.True(result.IsSuccessful);
    }
   
    [Fact]
    public void SharedAssemblyDoesNotReferenceControllers()
    {
        var result = 
            Types.InAssembly(_sharedAssembly) 
                .ShouldNot()
                .HaveDependencyOnAny(_presentationNameSpaces)
                .GetResult();

        Assert.True(result.IsSuccessful);
    }
    [Fact]
    public void SharedAssemblyDoesNotReferenceDomain()
    {
        var result = 
            Types.InAssembly(_sharedAssembly) 
                .ShouldNot()
                .HaveDependencyOnAny(_domainNamespaces)
                .GetResult();

        Assert.True(result.IsSuccessful);
    }
    [Fact]
    public void SharedAssemblyDoesNotReferencedData()
    {
        var result = 
            Types.InAssembly(_sharedAssembly) 
                .ShouldNot()
                .HaveDependencyOnAny(_dataNamespaces)
                .GetResult();

        Assert.True(result.IsSuccessful);
    }
}