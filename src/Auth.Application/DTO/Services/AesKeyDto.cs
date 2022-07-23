namespace Auth.Application.DTO.Services;

public record AesKeyDto
{
    /// <summary>
    /// CBC AES Key
    /// </summary>
    public string Key { get; init; }
    
    /// <summary>
    /// CBC AES Initial vectors
    /// </summary>
    public string IV { get; init; }

    /// <summary>
    /// Implicit Casting Operator
    /// AesKeyDto to tuple
    /// </summary>
    /// <param name="response"></param>
    /// <returns></returns>
    public static implicit operator (string key, string iv)(AesKeyDto response) => (response.Key, response.IV);
}