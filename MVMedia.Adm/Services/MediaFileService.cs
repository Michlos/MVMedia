﻿using MVMedia.Adm.Models;
using MVMedia.Adm.Services.Interfaces;
using System.Text.Json;

namespace MVMedia.Adm.Services;

public class MediaFileService : IMediaFileService
{
    private readonly IHttpClientFactory _clientFactory;


    private const string apiEndpoint = "/api/MediaFile/";
    private readonly JsonSerializerOptions _options;
    private MediaFileViewModel? mediaFileVM;
    private IEnumerable<MediaFileViewModel>? mediaFileListVM;
    public MediaFileService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory;
        _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }
    public async Task<IEnumerable<MediaFileViewModel>> GetAllMediaFiles()
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + "ListActiveMediaFiles");
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            mediaFileListVM = JsonSerializer.Deserialize<IEnumerable<MediaFileViewModel>>(apiResponse, _options);
        }
        else
            return null;

        return mediaFileListVM;
    }
    public async Task<IEnumerable<string>> GetAllMediaFileURI()
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + "LIstMediaUris");
        IEnumerable<string> uriList = null;
        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            uriList = JsonSerializer.Deserialize<IEnumerable<string>>(apiResponse, _options);
        }
        else
            return null;

        return uriList;
    }
    public async Task<MediaFileViewModel> AddMediaFile(MediaFileViewModel mediaFile)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(mediaFile.Title ?? ""), "Title");
        form.Add(new StringContent(mediaFile.Description ?? ""), "Description");
        form.Add(new StringContent(mediaFile.IsPublic.ToString()), "IsPublic");
        form.Add(new StringContent(mediaFile.ClientId.ToString()), "ClientId");

        // Adiciona o arquivo
        if (!string.IsNullOrEmpty(mediaFile.FileName) && mediaFile.FileSize > 0)
        {
            // Supondo que você tenha o arquivo físico disponível em algum local
            // Exemplo: mediaFile.FilePath ou similar
            var filePath = Path.Combine("CAMINHO_ONDE_ESTA_O_ARQUIVO", mediaFile.FileName);
            if (File.Exists(filePath))
            {
                var fileStream = File.OpenRead(filePath);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                form.Add(fileContent, "File", mediaFile.FileName);
            }
            else
            {
                throw new FileNotFoundException("Arquivo não encontrado para upload.", filePath);
            }
        }
        else
        {
            throw new ArgumentException("Arquivo inválido para upload.");
        }

        var response = await client.PostAsync(apiEndpoint + "AddMediaFile", form);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MediaFileViewModel>(apiResponse, _options);
            return result;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Erro ao adicionar arquivo de mídia: {error}");
        }
    }

    public Task<bool> DeleteMediaFile(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<MediaFileViewModel> GetMediaFileById(Guid id)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");
        var response = await client.GetAsync(apiEndpoint + $"GetMediaFileById/{id}");

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MediaFileViewModel>(apiResponse, _options);
            return result;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Erro ao buscar arquivo de mídia: {error}");
        }
    }

    public async Task<MediaFileViewModel> UpdateMediaFile(MediaFileViewModel mediaFile, string oldFileName)
    {
        var client = _clientFactory.CreateClient("MVMediaAPI");

        using var form = new MultipartFormDataContent();
        form.Add(new StringContent(mediaFile.Title ?? ""), "Title");
        form.Add(new StringContent(mediaFile.Description ?? ""), "Description");
        form.Add(new StringContent(mediaFile.IsPublic.ToString()), "IsPublic");
        form.Add(new StringContent(mediaFile.ClientId.ToString()), "ClientId");

        // Adiciona o arquivo se houver alteração
        if (!string.IsNullOrEmpty(mediaFile.FileName) && mediaFile.FileSize > 0)
        {
            var filePath = Path.Combine("CAMINHO_ONDE_ESTA_O_ARQUIVO", mediaFile.FileName);
            if (File.Exists(filePath))
            {
                var fileStream = File.OpenRead(filePath);
                var fileContent = new StreamContent(fileStream);
                fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/octet-stream");
                form.Add(fileContent, "File", mediaFile.FileName);
            }
            // Se não houver arquivo novo, não adiciona nada (mantém o arquivo antigo)
        }

        var response = await client.PutAsync(apiEndpoint + $"UpdateMediaFile/{mediaFile.Id}", form);

        if (response.IsSuccessStatusCode)
        {
            var apiResponse = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<MediaFileViewModel>(apiResponse, _options);
            return result;
        }
        else
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new ApplicationException($"Erro ao atualizar arquivo de mídia: {error}");
        }
    }


}
