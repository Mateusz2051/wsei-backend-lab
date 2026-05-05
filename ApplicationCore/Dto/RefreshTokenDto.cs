namespace ApplicationCore.Dto;

public record RefreshTokenDto(
    string AccessToken,
    string RefreshToken
);
