namespace Api.Models;

public class ApiResponse<T>
{
    // ASSUMPTION: I was unsure as to what the difference between the Message field & the Error field should be.
    // For the sake of this small demo app, I made Message just a generic "something went wrong" type of message,
    // and made Error provide a bit of detail as to what specifically went wrong.

    public T? Data { get; set; }
    public bool Success { get; set; } = true;
    public string Message { get; set; } = string.Empty;
    public string Error { get; set; } = string.Empty;

    // I added this method because, if we ever had to add (or remove) more fields to this ApiResponse object / change the values we set certain fields to,
    // we would only have one spot that would be responsible for creating ApiResponse<T> that we want to consider successful.
    public static ApiResponse<T> CreateSuccessfulApiResponse(T? data)
    {
        return new ApiResponse<T>
        {
            Success = true,
            Data = data,
            // Since Message & Error currently default to string.Empty, I felt no need to set those here as well.
        };
    }

    // Same philosophy as above method, except for the unsuccessful ApiResponse<T> case.
    public static ApiResponse<T> CreateUnsuccessfulApiResponse(string message, string errorMessage)
    {
        return new ApiResponse<T>
        {
            Success = false,
            Error = errorMessage,
            Message = message,
            // ASSUMPTION: We would never want to set the Data field if we get an unsuccessful ApiResponse.
        };
    }
}
