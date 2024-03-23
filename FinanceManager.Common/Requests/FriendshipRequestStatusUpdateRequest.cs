namespace FinanceManager.Common.Requests;

public record FriendshipRequestStatusUpdateRequest(Guid FriendshipId, bool Accepted) : BaseEditRequest(Id: FriendshipId);