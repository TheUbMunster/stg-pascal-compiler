(Program
	(ProgramHeading
		Program 'program'
		(Identifier
			Letter 'F'
			Letter 'i'
			Letter 'b'
			Letter 'o'
			Letter 'n'
			Letter 'a'
			Letter 'c'
			Letter 'c'
			Letter 'i'
			Letter 'S'
			Letter 'e'
			Letter 'r'
			Letter 'i'
			Letter 'e'
			Letter 's'
		)
	)
	Semicolon ';'
	(ProgramBlock
		(Block
			(LabelDeclarationPart)
			(ConstantDefinitionPart)
			(TypeDefinitionPart)
			(VariableDeclarationPart
				Var 'var'
				(VariableDeclaration
					(IdentifierList
						(Identifier
							Letter 'n'
						)
						Comma ','
						(Identifier
							Letter 'i'
						)
						Comma ','
						(Identifier
							Letter 't'
							Letter 'e'
							Letter 'r'
							Letter 'm'
							Digit '1'
						)
						Comma ','
						(Identifier
							Letter 't'
							Letter 'e'
							Letter 'r'
							Letter 'm'
							Digit '2'
						)
						Comma ','
						(Identifier
							Letter 'n'
							Letter 'e'
							Letter 'x'
							Letter 't'
							Letter 'T'
							Letter 'e'
							Letter 'r'
							Letter 'm'
						)
					)
					Colon ':'
					(TypeDenoter
						(TypeIdentifier
							(Identifier
								Letter 'i'
								Letter 'n'
								Letter 't'
								Letter 'e'
								Letter 'g'
								Letter 'e'
								Letter 'r'
							)
						)
					)
				)
				Semicolon ';'
			)
			(ProcedureAndFunctionDeclarationPart)
			(StatementPart
				(CompoundStatement
					Begin 'begin'
					(StatementSequence
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter number of terms: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter number of terms: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Enter number of terms: ''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(Ambiguous ActualParameter
												(ActualParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																		)
																	)
																)
															)
														)
													)
												)
												(ActualParameter
													(ProcedureIdentifier
														(Identifier
															Letter 'n'
														)
													)
												)
												(ActualParameter
													(FunctionIdentifier
														(Identifier
															Letter 'n'
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(ReadParameterList
											OpenParen '('
											(Ambiguous VariableAccess
												(VariableAccess
													(EntireVariable
														(VariableIdentifier
															(Identifier
																Letter 'n'
															)
														)
													)
												)
												(VariableAccess
													(ComponentVariable
														(FieldDesignator
															(FieldDesignatorIdentifier
																(Identifier
																	Letter 'n'
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous ReadlnParameterList
											(ReadlnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(ReadlnParameterList
												OpenParen '('
												(Ambiguous VariableAccess
													(VariableAccess
														(EntireVariable
															(VariableIdentifier
																(Identifier
																	Letter 'n'
																)
															)
														)
													)
													(VariableAccess
														(ComponentVariable
															(FieldDesignator
																(FieldDesignatorIdentifier
																	(Identifier
																		Letter 'n'
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Ambiguous Factor
																(Factor
																	(BoundIdentifier
																		(Identifier
																			Letter 'n'
																		)
																	)
																)
																(Factor
																	(Ambiguous VariableAccess
																		(VariableAccess
																			(EntireVariable
																				(VariableIdentifier
																					(Identifier
																						Letter 'n'
																					)
																				)
																			)
																		)
																		(VariableAccess
																			(ComponentVariable
																				(FieldDesignator
																					(FieldDesignatorIdentifier
																						(Identifier
																							Letter 'n'
																						)
																					)
																				)
																			)
																		)
																	)
																)
																(Factor
																	(UnsignedConstant
																		(ConstantIdentifier
																			(Identifier
																				Letter 'n'
																			)
																		)
																	)
																)
																(Factor
																	(FunctionDesignator
																		(FunctionIdentifier
																			(Identifier
																				Letter 'n'
																			)
																		)
																	)
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'r'
												Letter 'e'
												Letter 'a'
												Letter 'd'
												Letter 'l'
												Letter 'n'
											)
										)
										(Ambiguous WritelnParameterList
											(WritelnParameterList
												OpenParen '('
												(FileVariable
													(Ambiguous VariableAccess
														(VariableAccess
															(EntireVariable
																(VariableIdentifier
																	(Identifier
																		Letter 'n'
																	)
																)
															)
														)
														(VariableAccess
															(ComponentVariable
																(FieldDesignator
																	(FieldDesignatorIdentifier
																		(Identifier
																			Letter 'n'
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
											(WritelnParameterList
												OpenParen '('
												(WriteParameter
													(Expression
														(SimpleExpression
															(Term
																(Ambiguous Factor
																	(Factor
																		(BoundIdentifier
																			(Identifier
																				Letter 'n'
																			)
																		)
																	)
																	(Factor
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(Factor
																		(UnsignedConstant
																			(ConstantIdentifier
																				(Identifier
																					Letter 'n'
																				)
																			)
																		)
																	)
																	(Factor
																		(FunctionDesignator
																			(FunctionIdentifier
																				(Identifier
																					Letter 'n'
																				)
																			)
																		)
																	)
																)
															)
														)
													)
												)
												CloseParen ')'
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous AssignmentStatement
									(AssignmentStatement
										(Ambiguous VariableAccess
											(VariableAccess
												(EntireVariable
													(VariableIdentifier
														(Identifier
															Letter 't'
															Letter 'e'
															Letter 'r'
															Letter 'm'
															Digit '1'
														)
													)
												)
											)
											(VariableAccess
												(ComponentVariable
													(FieldDesignator
														(FieldDesignatorIdentifier
															(Identifier
																Letter 't'
																Letter 'e'
																Letter 'r'
																Letter 'm'
																Digit '1'
															)
														)
													)
												)
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															(UnsignedNumber
																(UnsignedInteger
																	(DigitSequence
																		Digit '0'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
									(AssignmentStatement
										(FunctionIdentifier
											(Identifier
												Letter 't'
												Letter 'e'
												Letter 'r'
												Letter 'm'
												Digit '1'
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															(UnsignedNumber
																(UnsignedInteger
																	(DigitSequence
																		Digit '0'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous AssignmentStatement
									(AssignmentStatement
										(Ambiguous VariableAccess
											(VariableAccess
												(EntireVariable
													(VariableIdentifier
														(Identifier
															Letter 't'
															Letter 'e'
															Letter 'r'
															Letter 'm'
															Digit '2'
														)
													)
												)
											)
											(VariableAccess
												(ComponentVariable
													(FieldDesignator
														(FieldDesignatorIdentifier
															(Identifier
																Letter 't'
																Letter 'e'
																Letter 'r'
																Letter 'm'
																Digit '2'
															)
														)
													)
												)
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															(UnsignedNumber
																(UnsignedInteger
																	(DigitSequence
																		Digit '1'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
									(AssignmentStatement
										(FunctionIdentifier
											(Identifier
												Letter 't'
												Letter 'e'
												Letter 'r'
												Letter 'm'
												Digit '2'
											)
										)
										Walrus ':='
										(Expression
											(SimpleExpression
												(Term
													(Factor
														(UnsignedConstant
															(UnsignedNumber
																(UnsignedInteger
																	(DigitSequence
																		Digit '1'
																	)
																)
															)
														)
													)
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(Ambiguous ProcedureStatement
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(ActualParameterList
											OpenParen '('
											(ActualParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Fibonacci Series:''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WriteParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Fibonacci Series:''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
									(ProcedureStatement
										(ProcedureIdentifier
											(Identifier
												Letter 'w'
												Letter 'r'
												Letter 'i'
												Letter 't'
												Letter 'e'
												Letter 'l'
												Letter 'n'
											)
										)
										(WritelnParameterList
											OpenParen '('
											(WriteParameter
												(Expression
													(SimpleExpression
														(Term
															(Factor
																(UnsignedConstant
																	CharacterString ''Fibonacci Series:''
																)
															)
														)
													)
												)
											)
											CloseParen ')'
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(StructuredStatement
								(RepetitiveStatement
									(ForStatement
										For 'for'
										(ControlVariable
											(EntireVariable
												(VariableIdentifier
													(Identifier
														Letter 'i'
													)
												)
											)
										)
										Walrus ':='
										(InitialValue
											(Expression
												(SimpleExpression
													(Term
														(Factor
															(UnsignedConstant
																(UnsignedNumber
																	(UnsignedInteger
																		(DigitSequence
																			Digit '1'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										To 'to'
										(FinalValue
											(Expression
												(SimpleExpression
													(Term
														(Ambiguous Factor
															(Factor
																(BoundIdentifier
																	(Identifier
																		Letter 'n'
																	)
																)
															)
															(Factor
																(Ambiguous VariableAccess
																	(VariableAccess
																		(EntireVariable
																			(VariableIdentifier
																				(Identifier
																					Letter 'n'
																				)
																			)
																		)
																	)
																	(VariableAccess
																		(ComponentVariable
																			(FieldDesignator
																				(FieldDesignatorIdentifier
																					(Identifier
																						Letter 'n'
																					)
																				)
																			)
																		)
																	)
																)
															)
															(Factor
																(UnsignedConstant
																	(ConstantIdentifier
																		(Identifier
																			Letter 'n'
																		)
																	)
																)
															)
															(Factor
																(FunctionDesignator
																	(FunctionIdentifier
																		(Identifier
																			Letter 'n'
																		)
																	)
																)
															)
														)
													)
												)
											)
										)
										Do 'do'
										(Statement
											(StructuredStatement
												(CompoundStatement
													Begin 'begin'
													(StatementSequence
														(Statement
															(SimpleStatement
																(Ambiguous ProcedureStatement
																	(ProcedureStatement
																		(ProcedureIdentifier
																			(Identifier
																				Letter 'w'
																				Letter 'r'
																				Letter 'i'
																				Letter 't'
																				Letter 'e'
																			)
																		)
																		(ActualParameterList
																			OpenParen '('
																			(Ambiguous ActualParameter
																				(ActualParameter
																					(Expression
																						(SimpleExpression
																							(Term
																								(Ambiguous Factor
																									(Factor
																										(BoundIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '1'
																											)
																										)
																									)
																									(Factor
																										(Ambiguous VariableAccess
																											(VariableAccess
																												(EntireVariable
																													(VariableIdentifier
																														(Identifier
																															Letter 't'
																															Letter 'e'
																															Letter 'r'
																															Letter 'm'
																															Digit '1'
																														)
																													)
																												)
																											)
																											(VariableAccess
																												(ComponentVariable
																													(FieldDesignator
																														(FieldDesignatorIdentifier
																															(Identifier
																																Letter 't'
																																Letter 'e'
																																Letter 'r'
																																Letter 'm'
																																Digit '1'
																															)
																														)
																													)
																												)
																											)
																										)
																									)
																									(Factor
																										(UnsignedConstant
																											(ConstantIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																									(Factor
																										(FunctionDesignator
																											(FunctionIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				(ActualParameter
																					(Ambiguous VariableAccess
																						(VariableAccess
																							(EntireVariable
																								(VariableIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																						(VariableAccess
																							(ComponentVariable
																								(FieldDesignator
																									(FieldDesignatorIdentifier
																										(Identifier
																											Letter 't'
																											Letter 'e'
																											Letter 'r'
																											Letter 'm'
																											Digit '1'
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				(ActualParameter
																					(ProcedureIdentifier
																						(Identifier
																							Letter 't'
																							Letter 'e'
																							Letter 'r'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																				(ActualParameter
																					(FunctionIdentifier
																						(Identifier
																							Letter 't'
																							Letter 'e'
																							Letter 'r'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			Comma ','
																			(ActualParameter
																				(Expression
																					(SimpleExpression
																						(Term
																							(Factor
																								(UnsignedConstant
																									CharacterString '' ''
																								)
																							)
																						)
																					)
																				)
																			)
																			CloseParen ')'
																		)
																	)
																	(ProcedureStatement
																		(ProcedureIdentifier
																			(Identifier
																				Letter 'w'
																				Letter 'r'
																				Letter 'i'
																				Letter 't'
																				Letter 'e'
																			)
																		)
																		(Ambiguous WriteParameterList
																			(WriteParameterList
																				OpenParen '('
																				(FileVariable
																					(Ambiguous VariableAccess
																						(VariableAccess
																							(EntireVariable
																								(VariableIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																						(VariableAccess
																							(ComponentVariable
																								(FieldDesignator
																									(FieldDesignatorIdentifier
																										(Identifier
																											Letter 't'
																											Letter 'e'
																											Letter 'r'
																											Letter 'm'
																											Digit '1'
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				Comma ','
																				(WriteParameter
																					(Expression
																						(SimpleExpression
																							(Term
																								(Factor
																									(UnsignedConstant
																										CharacterString '' ''
																									)
																								)
																							)
																						)
																					)
																				)
																				CloseParen ')'
																			)
																			(WriteParameterList
																				OpenParen '('
																				(WriteParameter
																					(Expression
																						(SimpleExpression
																							(Term
																								(Ambiguous Factor
																									(Factor
																										(BoundIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '1'
																											)
																										)
																									)
																									(Factor
																										(Ambiguous VariableAccess
																											(VariableAccess
																												(EntireVariable
																													(VariableIdentifier
																														(Identifier
																															Letter 't'
																															Letter 'e'
																															Letter 'r'
																															Letter 'm'
																															Digit '1'
																														)
																													)
																												)
																											)
																											(VariableAccess
																												(ComponentVariable
																													(FieldDesignator
																														(FieldDesignatorIdentifier
																															(Identifier
																																Letter 't'
																																Letter 'e'
																																Letter 'r'
																																Letter 'm'
																																Digit '1'
																															)
																														)
																													)
																												)
																											)
																										)
																									)
																									(Factor
																										(UnsignedConstant
																											(ConstantIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																									(Factor
																										(FunctionDesignator
																											(FunctionIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				Comma ','
																				(WriteParameter
																					(Expression
																						(SimpleExpression
																							(Term
																								(Factor
																									(UnsignedConstant
																										CharacterString '' ''
																									)
																								)
																							)
																						)
																					)
																				)
																				CloseParen ')'
																			)
																		)
																	)
																	(ProcedureStatement
																		(ProcedureIdentifier
																			(Identifier
																				Letter 'w'
																				Letter 'r'
																				Letter 'i'
																				Letter 't'
																				Letter 'e'
																			)
																		)
																		(Ambiguous WritelnParameterList
																			(WritelnParameterList
																				OpenParen '('
																				(FileVariable
																					(Ambiguous VariableAccess
																						(VariableAccess
																							(EntireVariable
																								(VariableIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																						(VariableAccess
																							(ComponentVariable
																								(FieldDesignator
																									(FieldDesignatorIdentifier
																										(Identifier
																											Letter 't'
																											Letter 'e'
																											Letter 'r'
																											Letter 'm'
																											Digit '1'
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				Comma ','
																				(WriteParameter
																					(Expression
																						(SimpleExpression
																							(Term
																								(Factor
																									(UnsignedConstant
																										CharacterString '' ''
																									)
																								)
																							)
																						)
																					)
																				)
																				CloseParen ')'
																			)
																			(WritelnParameterList
																				OpenParen '('
																				(WriteParameter
																					(Expression
																						(SimpleExpression
																							(Term
																								(Ambiguous Factor
																									(Factor
																										(BoundIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '1'
																											)
																										)
																									)
																									(Factor
																										(Ambiguous VariableAccess
																											(VariableAccess
																												(EntireVariable
																													(VariableIdentifier
																														(Identifier
																															Letter 't'
																															Letter 'e'
																															Letter 'r'
																															Letter 'm'
																															Digit '1'
																														)
																													)
																												)
																											)
																											(VariableAccess
																												(ComponentVariable
																													(FieldDesignator
																														(FieldDesignatorIdentifier
																															(Identifier
																																Letter 't'
																																Letter 'e'
																																Letter 'r'
																																Letter 'm'
																																Digit '1'
																															)
																														)
																													)
																												)
																											)
																										)
																									)
																									(Factor
																										(UnsignedConstant
																											(ConstantIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																									(Factor
																										(FunctionDesignator
																											(FunctionIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																					)
																				)
																				Comma ','
																				(WriteParameter
																					(Expression
																						(SimpleExpression
																							(Term
																								(Factor
																									(UnsignedConstant
																										CharacterString '' ''
																									)
																								)
																							)
																						)
																					)
																				)
																				CloseParen ')'
																			)
																		)
																	)
																)
															)
														)
														Semicolon ';'
														(Statement
															(SimpleStatement
																(Ambiguous AssignmentStatement
																	(AssignmentStatement
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 'n'
																							Letter 'e'
																							Letter 'x'
																							Letter 't'
																							Letter 'T'
																							Letter 'e'
																							Letter 'r'
																							Letter 'm'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 'n'
																								Letter 'e'
																								Letter 'x'
																								Letter 't'
																								Letter 'T'
																								Letter 'e'
																								Letter 'r'
																								Letter 'm'
																							)
																						)
																					)
																				)
																			)
																		)
																		Walrus ':='
																		(Expression
																			(SimpleExpression
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 't'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '1'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																				(AddingOperator
																					Plus '+'
																				)
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 't'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '2'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '2'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(AssignmentStatement
																		(FunctionIdentifier
																			(Identifier
																				Letter 'n'
																				Letter 'e'
																				Letter 'x'
																				Letter 't'
																				Letter 'T'
																				Letter 'e'
																				Letter 'r'
																				Letter 'm'
																			)
																		)
																		Walrus ':='
																		(Expression
																			(SimpleExpression
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 't'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																									Digit '1'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '1'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '1'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '1'
																									)
																								)
																							)
																						)
																					)
																				)
																				(AddingOperator
																					Plus '+'
																				)
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 't'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '2'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '2'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Semicolon ';'
														(Statement
															(SimpleStatement
																(Ambiguous AssignmentStatement
																	(AssignmentStatement
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 't'
																							Letter 'e'
																							Letter 'r'
																							Letter 'm'
																							Digit '1'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 't'
																								Letter 'e'
																								Letter 'r'
																								Letter 'm'
																								Digit '1'
																							)
																						)
																					)
																				)
																			)
																		)
																		Walrus ':='
																		(Expression
																			(SimpleExpression
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 't'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '2'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '2'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(AssignmentStatement
																		(FunctionIdentifier
																			(Identifier
																				Letter 't'
																				Letter 'e'
																				Letter 'r'
																				Letter 'm'
																				Digit '1'
																			)
																		)
																		Walrus ':='
																		(Expression
																			(SimpleExpression
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 't'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																									Digit '2'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 't'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																												Digit '2'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 't'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																													Digit '2'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 't'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																										Digit '2'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Semicolon ';'
														(Statement
															(SimpleStatement
																(Ambiguous AssignmentStatement
																	(AssignmentStatement
																		(Ambiguous VariableAccess
																			(VariableAccess
																				(EntireVariable
																					(VariableIdentifier
																						(Identifier
																							Letter 't'
																							Letter 'e'
																							Letter 'r'
																							Letter 'm'
																							Digit '2'
																						)
																					)
																				)
																			)
																			(VariableAccess
																				(ComponentVariable
																					(FieldDesignator
																						(FieldDesignatorIdentifier
																							(Identifier
																								Letter 't'
																								Letter 'e'
																								Letter 'r'
																								Letter 'm'
																								Digit '2'
																							)
																						)
																					)
																				)
																			)
																		)
																		Walrus ':='
																		(Expression
																			(SimpleExpression
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'e'
																									Letter 'x'
																									Letter 't'
																									Letter 'T'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 'n'
																												Letter 'e'
																												Letter 'x'
																												Letter 't'
																												Letter 'T'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 'n'
																													Letter 'e'
																													Letter 'x'
																													Letter 't'
																													Letter 'T'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'e'
																										Letter 'x'
																										Letter 't'
																										Letter 'T'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'e'
																										Letter 'x'
																										Letter 't'
																										Letter 'T'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																	(AssignmentStatement
																		(FunctionIdentifier
																			(Identifier
																				Letter 't'
																				Letter 'e'
																				Letter 'r'
																				Letter 'm'
																				Digit '2'
																			)
																		)
																		Walrus ':='
																		(Expression
																			(SimpleExpression
																				(Term
																					(Ambiguous Factor
																						(Factor
																							(BoundIdentifier
																								(Identifier
																									Letter 'n'
																									Letter 'e'
																									Letter 'x'
																									Letter 't'
																									Letter 'T'
																									Letter 'e'
																									Letter 'r'
																									Letter 'm'
																								)
																							)
																						)
																						(Factor
																							(Ambiguous VariableAccess
																								(VariableAccess
																									(EntireVariable
																										(VariableIdentifier
																											(Identifier
																												Letter 'n'
																												Letter 'e'
																												Letter 'x'
																												Letter 't'
																												Letter 'T'
																												Letter 'e'
																												Letter 'r'
																												Letter 'm'
																											)
																										)
																									)
																								)
																								(VariableAccess
																									(ComponentVariable
																										(FieldDesignator
																											(FieldDesignatorIdentifier
																												(Identifier
																													Letter 'n'
																													Letter 'e'
																													Letter 'x'
																													Letter 't'
																													Letter 'T'
																													Letter 'e'
																													Letter 'r'
																													Letter 'm'
																												)
																											)
																										)
																									)
																								)
																							)
																						)
																						(Factor
																							(UnsignedConstant
																								(ConstantIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'e'
																										Letter 'x'
																										Letter 't'
																										Letter 'T'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																									)
																								)
																							)
																						)
																						(Factor
																							(FunctionDesignator
																								(FunctionIdentifier
																									(Identifier
																										Letter 'n'
																										Letter 'e'
																										Letter 'x'
																										Letter 't'
																										Letter 'T'
																										Letter 'e'
																										Letter 'r'
																										Letter 'm'
																									)
																								)
																							)
																						)
																					)
																				)
																			)
																		)
																	)
																)
															)
														)
														Semicolon ';'
														(Statement
															(SimpleStatement
																(EmptyStatement)
															)
														)
													)
													End 'end'
												)
											)
										)
									)
								)
							)
						)
						Semicolon ';'
						(Statement
							(SimpleStatement
								(EmptyStatement)
							)
						)
					)
					End 'end'
				)
			)
		)
	)
	Dot '.'
)