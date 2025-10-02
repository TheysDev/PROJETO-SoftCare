namespace SoftCare.Dtos.Respostas;

public record ResumoRespostasDto(string CodigoDaPergunta, string TextoDaPergunta, List<ContagemDeRespostaDto> ContagemDetalhada);