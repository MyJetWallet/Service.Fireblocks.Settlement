namespace Service.Fireblocks.Settlement.Grpc.Models.Common
{
    public enum ErrorCode
    {
        Unknown,
        AlreadyExist,
        DoesNotExist,
        TransferIsRunning,
        ApiError
    }
}
