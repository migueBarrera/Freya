namespace FreyaApi.Repository.Tests.Services;

[TestFixture]
internal class ClientRepositoryShould
{
    private ClientRepositoryBuilder builder;

    private readonly string CLIENT_EMAIL = "migue@yopmail.com";

    [SetUp]
    public void SetUp()
    {
        builder = new ClientRepositoryBuilder();
    }

    [Test]
    public async Task CanCreateClient()
    {
        var service = builder.Build();

        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
        };

        var clientCreated = await service.Create(client);

        Assert.That(clientCreated, Is.Not.Null);
    }
    
    [Test]
    public async Task CanReadClientByEmail()
    {
        var service = builder.Build();

        
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        await service.Create(client);

        var clientDatabase = await service.GetClientByEmail(client.Email);
       
        Assert.Multiple(() =>
        {
            Assert.That(clientDatabase, Is.Not.Null);
            Assert.That(clientDatabase?.Id, Is.EqualTo(client.Id));
            Assert.That(clientDatabase?.Email, Is.EqualTo(client.Email));
        });
    }
    
    [Test]
    public async Task CanReadClientById()
    {
        var service = builder.Build();

        
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        await service.Create(client);

        var clientDatabase = service.GetClientById(client.Id);
       
        Assert.Multiple(() =>
        {
            Assert.That(clientDatabase, Is.Not.Null);
            Assert.That(clientDatabase?.Id, Is.EqualTo(client.Id));
            Assert.That(clientDatabase?.Email, Is.EqualTo(client.Email));
        });
    }

    [Test]
    public async Task CanReadClientIdByEmail()
    {
        var service = builder.Build();

        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        await service.Create(client);

        var clientDatabaseId = await service.GetClientIdByEmail(client.Email);

        Assert.Multiple(() =>
        {
            Assert.That(clientDatabaseId, Is.Not.Null);
            Assert.That(clientDatabaseId, Is.EqualTo(client.Id));
        });
    }

    [Test]
    public async Task GetClientByIdWithClinicRelation()
    {
        var service = builder.Build();
        
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = Guid.NewGuid(),
        };

        await service.Create(client);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.SaveChanges();

        var clientDatabase = service.GetClientByIdWithClinicRelation(client.Id, clinicClient.ClinicId);
        var clinicRelation = clientDatabase?.ClinicClients?.FirstOrDefault();

        Assert.Multiple(() =>
        {
            Assert.That(clientDatabase, Is.Not.Null);
            Assert.That(clinicRelation, Is.Not.Null);
            Assert.That(clientDatabase?.Id, Is.EqualTo(client.Id));
            Assert.That(clientDatabase?.Email, Is.EqualTo(client.Email));
            Assert.That(clinicRelation?.ClinicId, Is.EqualTo(clinicClient.ClinicId));
            Assert.That(clinicRelation?.ClientId, Is.EqualTo(clinicClient.ClientId));
        });
    }
    
    [Test]
    public async Task GetClientByIdWithClinicRelationWithMoreData()
    {
        var service = builder.Build();
        
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };
        
        var client2 = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };
        
        var client3 = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = Guid.NewGuid(),
        };
        
        var clinicClient2 = new ClinicClient()
        {
            ClientId = client2.Id,
            ClinicId = Guid.NewGuid(),
        };
        
        var clinicClient3 = new ClinicClient()
        {
            ClientId = client3.Id,
            ClinicId = Guid.NewGuid(),
        };

        await service.Create(client);
        await service.Create(client2);
        await service.Create(client3);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.ClinicClients?.Add(clinicClient2);
        builder.dbContext?.ClinicClients?.Add(clinicClient3);
        builder.dbContext?.SaveChanges();

        var clientDatabase = service.GetClientByIdWithClinicRelation(client2.Id, clinicClient2.ClinicId);
        var clinicRelation = clientDatabase?.ClinicClients?.FirstOrDefault();

        Assert.Multiple(() =>
        {
            Assert.That(clientDatabase, Is.Not.Null);
            Assert.That(clinicRelation, Is.Not.Null);
            Assert.That(clientDatabase?.Id, Is.EqualTo(client2.Id));
            Assert.That(clientDatabase?.Email, Is.EqualTo(client2.Email));
            Assert.That(clinicRelation?.ClinicId, Is.EqualTo(clinicClient2.ClinicId));
            Assert.That(clinicRelation?.ClientId, Is.EqualTo(clinicClient2.ClientId));
        });
    }
    
    [Test]
    public async Task GetClientByIdWithClinicRelationWithMoreDataRelations()
    {
        var service = builder.Build();
        
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };
        
        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = Guid.NewGuid(),
        };
        
        var clinicClient2 = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = Guid.NewGuid(),
        };
        
        var clinicClient3 = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = Guid.NewGuid(),
        };

        await service.Create(client);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.ClinicClients?.Add(clinicClient2);
        builder.dbContext?.ClinicClients?.Add(clinicClient3);
        builder.dbContext?.SaveChanges();

        var clientDatabase = service.GetClientByIdWithClinicRelation(client.Id, clinicClient2.ClinicId);
        var clinicRelation = clientDatabase?.ClinicClients?.FirstOrDefault();

        Assert.Multiple(() =>
        {
            Assert.That(clientDatabase, Is.Not.Null);
            Assert.That(clinicRelation, Is.Not.Null);
            Assert.That(clientDatabase?.Id, Is.EqualTo(client.Id));
            Assert.That(clientDatabase?.Email, Is.EqualTo(client.Email));
            Assert.That(clinicRelation?.ClinicId, Is.EqualTo(clinicClient2.ClinicId), message: "The clinic recived is incorrect");
            Assert.That(clinicRelation?.ClientId, Is.EqualTo(clinicClient2.ClientId));
        });
    }

    [Test]
    public async Task CanCalculateSizeCorrectlyOnlyWithOneVideo()
    {
        var service = builder.Build();
        Guid clinicId = Guid.NewGuid();
        var size = 100L;
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };
        
        var video100 = new VideoTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = clinicId,
        };

        await service.Create(client);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.Videos?.Add(video100);
        builder.dbContext?.SaveChanges();

        var sizeForClient1 = await service.GetSizeFromClientAndClinic(clinicClient.ClientId, clinicClient.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(sizeForClient1, Is.EqualTo(size));
        });
    }

    [Test]
    public async Task CanCalculateSizeCorrectlyOnlyWithOneImage()
    {
        var service = builder.Build();
        Guid clinicId = Guid.NewGuid();
        var size = 100L;
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        var image100 = new UltrasoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = clinicId,
        };

        await service.Create(client);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.Ultrasounds?.Add(image100);
        builder.dbContext?.SaveChanges();

        var sizeForClient1 = await service.GetSizeFromClientAndClinic(clinicClient.ClientId, clinicClient.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(sizeForClient1, Is.EqualTo(size));
        });
    }
    
    [Test]
    public async Task CanCalculateSizeCorrectlyOnlyWithOneSound()
    {
        var service = builder.Build();
        Guid clinicId = Guid.NewGuid();
        var size = 100L;
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        var sound100 = new SoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = clinicId,
        };

        await service.Create(client);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.Sounds?.Add(sound100);
        builder.dbContext?.SaveChanges();

        var sizeForClient1 = await service.GetSizeFromClientAndClinic(clinicClient.ClientId, clinicClient.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(sizeForClient1, Is.EqualTo(size));
        });
    }

    [Test]
    public async Task CanCalculateSizeCorrectlyMultiItems()
    {
        var service = builder.Build();
        Guid clinicId = Guid.NewGuid();
        var size = 100L;
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        var sound100 = new SoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var image100 = new UltrasoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };
        
        var video100 = new VideoTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = clinicId,
        };

        await service.Create(client);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.Sounds?.Add(sound100);
        builder.dbContext?.Videos?.Add(video100);
        builder.dbContext?.Ultrasounds?.Add(image100);
        builder.dbContext?.SaveChanges();

        var sizeForClient1 = await service.GetSizeFromClientAndClinic(clinicClient.ClientId, clinicClient.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(sizeForClient1, Is.EqualTo(size * 3));
        });
    }
    
    [Test]
    public async Task CanCalculateSizeCorrectlyMultiItemsAndMultipleClinicsForOneClient()
    {
        var service = builder.Build();
        Guid clinicId = Guid.NewGuid();
        Guid clinicId2 = Guid.NewGuid();
        var size = 100L;
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        var sound100 = new SoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var image100 = new UltrasoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };
        
        var video100 = new VideoTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };
        
        var video100Clinic2 = new VideoTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId2,
            ClientId = client.Id,
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = clinicId,
        };
        
        var clinicClient2 = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = clinicId2,
        };

        await service.Create(client);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.ClinicClients?.Add(clinicClient2);
        builder.dbContext?.Sounds?.Add(sound100);
        builder.dbContext?.Videos?.Add(video100);
        builder.dbContext?.Videos?.Add(video100Clinic2);
        builder.dbContext?.Ultrasounds?.Add(image100);
        builder.dbContext?.SaveChanges();

        var sizeForClient1ForClinic1 = await service.GetSizeFromClientAndClinic(clinicClient.ClientId, clinicClient.ClinicId);
        var sizeForClient1ForClinic2 = await service.GetSizeFromClientAndClinic(clinicClient2.ClientId, clinicClient2.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(sizeForClient1ForClinic1, Is.EqualTo(size * 3));
            Assert.That(sizeForClient1ForClinic2, Is.EqualTo(size));
        });
    }

    [Test]
    public async Task CanCalculateSizeCorrectlyMultiItemsAndMultipleClinicsForTwoClient()
    {
        var service = builder.Build();
        Guid clinicId = Guid.NewGuid();
        var size = 100L;
        var client = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };
        
        var client2 = new ClientTable()
        {
            Email = CLIENT_EMAIL,
            Id = Guid.NewGuid(),
        };

        var sound100 = new SoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var image100 = new UltrasoundTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };

        var video100 = new VideoTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client.Id,
        };
        
        var video100Client2 = new VideoTable()
        {
            Size = size,
            Id = Guid.NewGuid(),
            ClinicId = clinicId,
            ClientId = client2.Id,
        };

        var clinicClient = new ClinicClient()
        {
            ClientId = client.Id,
            ClinicId = clinicId,
        };

        var clinicClient2 = new ClinicClient()
        {
            ClientId = client2.Id,
            ClinicId = clinicId,
        };

        await service.Create(client);
        await service.Create(client2);
        builder.dbContext?.ClinicClients?.Add(clinicClient);
        builder.dbContext?.ClinicClients?.Add(clinicClient2);
        builder.dbContext?.Sounds?.Add(sound100);
        builder.dbContext?.Videos?.Add(video100);
        builder.dbContext?.Videos?.Add(video100Client2);
        builder.dbContext?.Ultrasounds?.Add(image100);
        builder.dbContext?.SaveChanges();

        var sizeForClient1ForClinic1 = await service.GetSizeFromClientAndClinic(clinicClient.ClientId, clinicClient.ClinicId);
        var sizeForClient2ForClinic1 = await service.GetSizeFromClientAndClinic(clinicClient2.ClientId, clinicClient2.ClinicId);

        Assert.Multiple(() =>
        {
            Assert.That(sizeForClient1ForClinic1, Is.EqualTo(size * 3));
            Assert.That(sizeForClient2ForClinic1, Is.EqualTo(size));
        });
    }
}
