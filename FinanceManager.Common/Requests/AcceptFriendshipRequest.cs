namespace FinanceManager.Core.Requests;

public record AcceptFriendshipRequest(Guid FriendshipId, bool Accepted) : BaseEditRequest(FriendshipId);