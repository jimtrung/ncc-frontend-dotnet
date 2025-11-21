namespace Theater_Management_FE.DTOs;

public record MovieActorsRequest(Guid MovieId, List<Guid> ActorIds);
