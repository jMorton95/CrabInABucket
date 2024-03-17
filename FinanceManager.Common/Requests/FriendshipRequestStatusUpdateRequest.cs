namespace FinanceManager.Core.Requests;

public record FriendshipRequestStatusUpdateRequest(Guid FriendshipId, bool Accepted) : BaseEditRequest(Id: FriendshipId);